using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using quiz_oj.Configs.Exceptions;
using quiz_oj.Entities;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.User;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Controllers
{
    [Route("api/oj")]
    public class OjController : Controller
    {
        private IOjService ojService;
        private SessionUtils sessionUtils;
        
        public OjController(IOjService ojService, SessionUtils sessionUtils)
        {
            this.ojService = ojService;
            this.sessionUtils = sessionUtils;
        }
        
        [HttpGet("rank/{page}")]
        public async Task<List<OjPassCount>> OjRank([FromRoute] int page)
        {
            return await ojService.GetRank(page);
        }

        [HttpGet("list/{page}")]
        public async Task<List<OjQuestion>> ListOjQuestions([FromRoute] int page)
        {
            var userInfo = sessionUtils.GetCachedUserInfo(HttpContext);
            return await ojService.ListQuestionSummary(userInfo?.Id, page);
        }

        [HttpGet("{ojQuestionId}")]
        public async Task<OjQuestion> GetOjQuestion([FromRoute] string ojQuestionId)
        {
            var q = await ojService.GetQuestion(ojQuestionId);
            if (q == null)
            {
                throw new UserException("OjQuestionId 不存在！");
            }
            return q;
        }

        [HttpPost("submit")]
        public async Task<OjResult> SubmitCode([FromBody] OjSubmission submission)
        {
            async Task<OjResult> execute(UserInfo userInfo)
            {
                const string ojStatus = "oj_status";
                const string pending = "pending";
                const string over = "over";
                var status = sessionUtils.GetString(HttpContext, ojStatus);
                switch (status)
                {
                    case "pending":
                        return new OjResult { Pending = true, Pass = false, Failed = false};
                    case null:
                    case "over":
                        //"over" or null means this submit should be a new request
                        var t = ojService.PendResult(userInfo.Id, submission.OjQuestionId, submission.Code);
                        sessionUtils.SetString(HttpContext, ojStatus, pending);
                        var res = await t;
                        if (res.Pass)
                        {
                            await ojService.SaveSuccess(userInfo.Id, submission.OjQuestionId);
                        }
                        sessionUtils.SetString(HttpContext, ojStatus, over);
                        return res;
                    default:
                        throw new NonUserException("How could this happen ???");
                }
            }
            var userInfo = sessionUtils.GetCachedUserInfo(HttpContext);
            if (userInfo == null)
            {
                throw new UserException("用户未登录！");
            }

            var ojResult = await execute(userInfo);
            if (ojResult.Pending)
            {
                return ojResult;
            }
            
            if (ojResult.Pass)
            {
                await ojService.AddSubmitRecord(userInfo.Id, submission.OjQuestionId, submission.Code, "Pass All Test Cases");
            }else if (ojResult.Failed)
            {
                await ojService.AddSubmitRecord(userInfo.Id, submission.OjQuestionId, submission.Code, "Compilation Failed");
            }else if (!ojResult.Failed)
            {
                await ojService.AddSubmitRecord(userInfo.Id, submission.OjQuestionId, submission.Code, "Can't Pass All Test Cases");
            }

            return ojResult;
        }

        [HttpGet("submitRecords/{page}")]
        public async Task<List<OjSubmitRecord>> GetSubmitRecordsList([FromRoute] int page)
        {
            var userInfo = RaiseIfNotLoggedIn();
            return await ojService.GetSubmitRecordList(userInfo.Id, page);
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

    }
}