using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.Quiz;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Service.impls
{
    public class UploadService : IUploadService
    {
        private IOjDao ojDao;
        private IQuizDao quizDao;

        public UploadService(IOjDao ojDao, IQuizDao quizDao)
        {
            this.ojDao = ojDao;
            this.quizDao = quizDao;
        }
        
        public async Task<bool> UploadQuiz(QuizQuestion question)
        {
            for (var i = 0; i < question.OptionDTOList.Count; i++)
            {
                question.OptionDTOList[i].Id = i + 1;
            }
            return await quizDao.UploadQuiz(question);
        }

        public async Task<bool> UploadOj(OjTestCaseSet ojTestCaseSet)
        {
            if (!ojTestCaseSet.IsValidTestSet()) return false;
            
            var question = new OjQuestion
            {
                Content = ojTestCaseSet.Content,
                CreateTime = DateTime.Now,
                Difficulty = ojTestCaseSet.Difficulty,
                Title = ojTestCaseSet.Title,
                Code = ojTestCaseSet.OjCode
            };
            var testCaseTable = new OjTestCaseTable
            {
                TestCaseSetJson = JsonConvert.SerializeObject(ojTestCaseSet) 
            };
            await ojDao.UploadOj(question, testCaseTable);
            return true;
        }

        public async Task<bool> FixOjOrderId()
        {
            return await ojDao.FixOjOrderId();
        }
    }
}