using Microsoft.AspNetCore.Mvc;

namespace quiz_oj.Controllers
{
    public class IndexController : Controller
    {
        [Route("/")]
        public RedirectResult RedirectToIndex()
        {
            return Redirect("templates/index.html");
        }
        
    }
}