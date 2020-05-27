using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities.Review;

namespace quiz_oj.Dao.impls
{
    public class ReviewDao : IReviewDao
    {
        private QOJDBContext dbContext;
        
        public ReviewDao(QOJDBContext qojdbContext)
        {
            dbContext = qojdbContext;
        }
        
        public async Task<List<OjReview>> GetOjReview(string ojId, int page)
        {
            return await dbContext.OjReviews
                .Where(r => r.OjId == ojId)
                .Include(r => r.UserInfo)
                .OrderByDescending(r => r.CreateTime)
                .Skip((page - 1) * 10).Take(10).ToListAsync();
        }

        public async Task<bool> AddOjReview(string ojId, string userId, string review)
        {
            await dbContext.OjReviews.AddAsync(new OjReview
            {
                OjId = ojId,
                UserId = userId,
                Review = review,
                CreateTime = DateTime.Now
            });
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}