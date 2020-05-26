using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Entities.Review;

namespace quiz_oj.Service.interfaces
{
    public interface IReviewService
    {
        Task<List<OjReview>> GetOjReview(string ojId, int page);
        Task<bool> AddOjReview(string ojQuestionId, string userId, string review);
    }
}