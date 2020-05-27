using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.User;

namespace quiz_oj.Dao.interfaces
{
    public interface IOjDao
    {
        Task<List<OjPassCount>> GetRank(int page);
        Task<List<OjQuestion>> ListQuestionSummary(string userId, int page);
        Task<OjQuestion> GetQuestion(string id);
        Task<bool> SaveSuccess(string userId, string ojQuestionId);
        Task<string> UploadOj(OjQuestion question, OjTestCaseTable ojTestCaseTable);
        Task<bool> FixOjOrderId();
        Task<string> GetTestCase(string ojQuestionId);

        Task<bool> AddSubmitRecord(string userId, string ojId, string code, string info);

        Task<List<OjSubmitRecord>> GetSubmitRecordList(string userId, int page);
    }
}