using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using quiz_oj.Configs.Exceptions;
using quiz_oj.Entities;
using quiz_oj.Entities.Quiz;
using quiz_oj.Entities.User;
using quiz_oj.Service.impls;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Controllers
{
    [Route("api/quiz")]
    public class QuizController : Controller
    {
        //Answering代表当前正在答题
        //Over表示前面一轮答题结束了，只有当状态为Over时，查到的CurrScore才是最终成绩
        private const string QuizStatus = "quizStatus";
        private const string Answering = "answering";
        private const string Over = "over";

        private const string CurrScore = "currScore";
        private const string CurrQuestionId = "currQuestionId";
        private const string QuizStartTime = "quizStartTime";
        private const string QuizPreviousQuestionList = "quizPreviousQuestionList";
        private const int AnsweringSeconds = 60;
        private const int ScoreForEach = 5;

        private IQuizService quizService;
        private SessionUtils sessionUtils;
        
        public QuizController(IQuizService quizService, SessionUtils sessionUtils)
        {
            this.quizService = quizService;
            this.sessionUtils = sessionUtils;
        }
        
        [HttpGet("rank/{page}")]
        public async Task<List<QuizScore>> QuizRank([FromRoute] int page)
        {
            return await quizService.Rank(page);
        }

        [HttpPost("start")]
        public async Task<QuizStatus> StartQuiz()
        {
            var userInfo = RaiseIfNotLoggedIn();
            var status = sessionUtils.GetString(HttpContext, QuizStatus);

            async Task<QuizStatus> AnsweringDelegate()
            {
                var currScore = sessionUtils.GetInt(HttpContext, CurrScore);
                var currQuestionId = sessionUtils.GetString(HttpContext, CurrQuestionId);
                if (!currScore.HasValue || currQuestionId == null)
                {
                    //若已经丢失上次的分数和问题id，则直接重置
                    InitQuizStatus();
                    return new QuizStatus {Ok = true, ContinueLast = false};
                }

                int timeLeft;
                if ((timeLeft = TimeLeft()) <= 0)
                {
                    //已超时
                    var saved = await quizService.SaveScore(userInfo.Id, currScore.Value);
                    InitQuizStatus();
                    return new QuizStatus {Ok = true, AutomaticSubmitted = true};
                }
                else
                {
                    //继续上次答题
                    //把上次的问题查询到
                    var lastQuestion = await quizService.GetQuestion(currQuestionId);

                    return new QuizStatus {Ok = false, LastRemainingSecs = timeLeft, ContinueLast = true, LastScore = currScore.Value, LastQuizQuestion = lastQuestion};
                }
            }

            QuizStatus OverOrNullDelegate()
            {
                InitQuizStatus();
                return new QuizStatus {Ok = true, ContinueLast = false};
            }

            switch (status)
            {
                case Answering:
                    return await AnsweringDelegate();
                case Over:
                case null:
                    return OverOrNullDelegate();
                default:
                    throw new NonUserException("How could this happen ???");
            }
        }

        [HttpGet("list")]
        public async Task<QuizQuestion> NextQuestionInList()
        {
            RaiseIfNotLoggedIn();
            RaiseIfTimeOut();
            var previousQuestionList = sessionUtils.GetString(HttpContext, QuizPreviousQuestionList);
            var list = previousQuestionList == null
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(previousQuestionList);
            var question = await quizService.GetRandomQuestionBaseOnPreviousQuestionList(list);
            list.Add(question.Id);
            sessionUtils.SetString(HttpContext, QuizPreviousQuestionList, JsonConvert.SerializeObject(list));
            sessionUtils.SetString(HttpContext, CurrQuestionId, question.Id);
            return question;
        }

        [HttpPost("validate")]
        public async Task<QuizValidation> Validate([FromBody] QuizSubmit quizSubmit)
        {
            RaiseIfNotLoggedIn();
            RaiseIfTimeOut();
            var currQuestionId = sessionUtils.GetString(HttpContext, CurrQuestionId);
            var currScore = sessionUtils.GetInt(HttpContext, CurrScore);
            if (currQuestionId == null || !currScore.HasValue)
            {
                throw new UserException("不在答题状态！");
            }
            var correct = await quizService.Validate(currQuestionId, quizSubmit);
            if (correct)
            {
                sessionUtils.SetInt(HttpContext, CurrScore, currScore.Value + ScoreForEach);
                ResetStartTime();
            }
            return new QuizValidation
            {
                Correct = correct,
            };
        }
        
        [HttpPost("result")]
        public async Task<QuizResult> ResultScore()
        {
            //End Quiz Session, Set QuizStatus = Over
            var userInfo = RaiseIfNotLoggedIn();
            var currScore = sessionUtils.GetInt(HttpContext, CurrScore);
            if (!currScore.HasValue)
            {
                throw new UserException("未答题！");
            } 
            await quizService.SaveScore(userInfo.Id, currScore.Value);
            sessionUtils.SetString(HttpContext, QuizStatus, Over);
            var res = new QuizResult { Score = currScore.Value };
            ClearSessions();
            return res;
        }

        private UserInfo RaiseIfNotLoggedIn()
        {
            var userInfo = sessionUtils.GetCachedUserInfo(HttpContext);
            if (userInfo == null)
            {
                throw new UserException("用户未登录！");
            }
            return userInfo;
        }

        private DateTime ParseTime(string time)
        {
            var fileTime = long.Parse(time);
            return DateTime.FromFileTime(fileTime);
        }

        private int TimeLeft()
        {
            var startTime = ParseTime(sessionUtils.GetString(HttpContext, QuizStartTime));
            var period = DateTime.Now.Subtract(startTime).TotalMilliseconds;
            return (int)((AnsweringSeconds * 1000.0 - period) / 1000.0);
        }

        private void ClearSessions()
        {
            sessionUtils.ClearString(HttpContext, QuizStatus);
            sessionUtils.ClearInt(HttpContext, CurrScore);
            sessionUtils.ClearInt(HttpContext, CurrQuestionId);
            sessionUtils.ClearString(HttpContext, QuizStartTime);
            sessionUtils.ClearString(HttpContext, QuizPreviousQuestionList);
        }
        
        private void RaiseIfTimeOut()
        {
            if (TimeLeft() <= 0)
            {
                throw new UserException("已超时！", -2);
            }
        }

        private void ResetStartTime()
        {
            sessionUtils.SetString(HttpContext, QuizStartTime, DateTime.Now.ToFileTime().ToString());
        }
        
        private void InitQuizStatus()
        {
            sessionUtils.SetString(HttpContext, QuizStatus, Answering);
            sessionUtils.SetString(HttpContext, QuizStartTime, DateTime.Now.ToFileTime().ToString());
            sessionUtils.SetInt(HttpContext, CurrScore, 0);
        }

    }
}