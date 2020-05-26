using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities.Review;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Service.impls
{
    public class ReviewService : IReviewService
    {
        private IReviewDao reviewDao;
        
        public ReviewService(IReviewDao reviewDao)
        {
            this.reviewDao = reviewDao;
        }
        
        public async Task<List<OjReview>> GetOjReview(string ojId, int page)
        {
            return await reviewDao.GetOjReview(ojId, page);
        }

        public async Task<bool> AddOjReview(string ojQuestionId, string userId, string review)
        {
            return await reviewDao.AddOjReview(ojQuestionId, userId, review);
        }
    }
}