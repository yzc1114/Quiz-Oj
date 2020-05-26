using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using quiz_oj.Dao;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.Quiz;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : Controller
    {
        private IUploadService uploadService;

        public UploadController(IUploadService uploadService, QOJDBContext qojdbContext)
        {
            this.uploadService = uploadService;
        }
        
        [HttpPost("quiz")]
        public async Task<bool> UploadQuiz([FromBody] QuizQuestion question)
        {
            return await uploadService.UploadQuiz(question);
        }

        [HttpPost("oj")]
        public async Task<bool> UploadOj([FromBody] OjTestCaseSet ojTestCaseSet)
        {
            return await uploadService.UploadOj(ojTestCaseSet);
        }
        
        [HttpPost("ojString")]
        public async Task<bool> UploadOjString([FromBody] string ojTestCaseSet)
        {
            var o = JsonConvert.DeserializeObject<OjTestCaseSet>(ojTestCaseSet);
            return await uploadService.UploadOj(o);
        }

        [HttpGet("fixOjOrderId")]
        public async Task<bool> FixOjOrderId()
        {
            return await uploadService.FixOjOrderId();
        }

    }
}