using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using quiz_oj.Dao.interfaces;
using quiz_oj.DynamicCodeExecutor;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.User;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Service.impls
{
    public class OjService : IOjService
    {

        private readonly IOjDao ojDao;
        private readonly CenterScheduler centerScheduler;

        public OjService(IOjDao ojDao, CenterScheduler centerScheduler)
        {
            this.ojDao = ojDao;
            this.centerScheduler = centerScheduler;
        }
        
        public async Task<List<OjPassCount>> GetRank(int page)
        {
            return await ojDao.GetRank(page);
        }

        public async Task<List<OjQuestion>> ListQuestionSummary(string userId, int page)
        {
            return await ojDao.ListQuestionSummary(userId, page);
        }

        public async Task<OjQuestion> GetQuestion(string id)
        {
            return await ojDao.GetQuestion(id);
        }

        public async Task<OjResult> PendResult(string identifier, string questionId, string code)
        {
            var testCaseJson = await ojDao.GetTestCase(questionId);
            var ojTestCaseSet = JsonConvert.DeserializeObject<OjTestCaseSet>(testCaseJson);
            return await centerScheduler.PendCode(identifier, code, ojTestCaseSet);
        }

        public async Task<bool> SaveSuccess(string userId, string questionId)
        {
            return await ojDao.SaveSuccess(userId, questionId);
        }
    }
}