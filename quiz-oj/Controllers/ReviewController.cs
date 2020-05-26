using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using quiz_oj.Entities.Review;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Controllers
{
    [Route("api/review")]
    public class ReviewController : Controller
    {
        private IReviewService reviewService;
        private SessionUtils sessionUtils;
        
        public ReviewController(IReviewService reviewService, SessionUtils sessionUtils)
        {
            this.reviewService = reviewService;
            this.sessionUtils = sessionUtils;
        }
        
        [HttpGet("getReview/{ojId}/{page}")]
        public async Task<List<OjReview>> GetOjReview([FromRoute] string ojId, [FromRoute] int page)
        {
            return await reviewService.GetOjReview(ojId, page);
        }

        [HttpGet("addReview/{ojId}/{review}")]
        public async Task<bool> AddOjReview([FromRoute]string ojId, [FromRoute] string review)
        {
            if (review.Length > 100)
            {
                return false;
            }

            var userInfo = sessionUtils.GetCachedUserInfo(HttpContext);
            if(userInfo == null)
            {
                return false;
            }
            return await reviewService.AddOjReview(ojId, userInfo.Id, review);
        }
    }
}