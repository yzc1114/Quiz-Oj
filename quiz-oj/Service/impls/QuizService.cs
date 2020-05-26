using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities.Quiz;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Service.impls
{
    public class QuizService : IQuizService
    {
        private IQuizDao quizDao;
        private Random random = new Random();

        private int? cachedQuestionCount;

        public QuizService(IQuizDao quizDao)
        {
            this.quizDao = quizDao;
        }
        public async Task<List<QuizScore>> Rank(int page)
        {
            return await quizDao.Rank(page);
        }

        public async Task<QuizQuestion> GetQuestion(string questionId)
        {
            var question = await quizDao.GetQuestion(questionId);
            HideCorrect(question);
            return question;
        }

        public async Task<bool> Validate(string quizId, QuizSubmit quizSubmit)
        {
            return await quizDao.Validate(quizId, quizSubmit);
        }

        public async Task<QuizQuestion> GetRandomQuestionBaseOnPreviousQuestionList(List<string> previousQuestionList)
        {
            cachedQuestionCount ??= await quizDao.GetQuestionCount();
            previousQuestionList.Sort();
            //loop to find not in previousQuestionList
            var skip = random.Next(cachedQuestionCount.Value - 1);
            var question = await quizDao.GetQuestionThatSkip(skip);
            HideCorrect(question);
            return question;
        }

        private static void HideCorrect(QuizQuestion question)
        {
            foreach (var quizOption in question.OptionDTOList)
            {
                quizOption.Correct = null;
            }
        }

        public async Task<bool> SaveScore(string userId, int score)
        {
            return await quizDao.SetHighScore(userId, score);
        }
    }
}