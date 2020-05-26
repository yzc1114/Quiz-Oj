using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.User;

namespace quiz_oj.Service.interfaces
{
    public interface IOjService
    {
        Task<List<OjPassCount>> GetRank(int page);
        Task<List<OjQuestion>> ListQuestionSummary(string userId, int page);
        Task<OjQuestion> GetQuestion(string id);

        Task<OjResult> PendResult(string identifier, string questionId, string code);

        Task<bool> SaveSuccess(string userId, string questionId);
    }
}