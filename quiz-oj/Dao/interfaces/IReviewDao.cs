using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Entities.Review;

namespace quiz_oj.Dao.interfaces
{
    public interface IReviewDao
    {
        Task<List<OjReview>> GetOjReview(string ojId, int page);
        Task<bool> AddOjReview(string ojId, string userId, string review);
    }
}