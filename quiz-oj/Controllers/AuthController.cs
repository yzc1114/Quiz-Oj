using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiz_oj.Entities;
using quiz_oj.Entities.User;
using quiz_oj.Service.interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace quiz_oj.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AuthController : Controller
    {

        private IAuthService authService;
        private SessionUtils sessionUtils;
        
        public AuthController(IAuthService authService, SessionUtils sessionUtils)
        {
            this.authService = authService;
            this.sessionUtils = sessionUtils;
        }
        
        [HttpPost("login")]
        public async Task<bool> Login([FromBody] UserInfo userInfo)
        {
            var cached = sessionUtils.GetCachedUserInfo(HttpContext);
            if (cached != null)
            {
                //已经登录
                return true;
            }
            var res = await authService.ValidateUserInfo(userInfo);
            if (res == null)
            {
                return false;
            }
            sessionUtils.SetSessionUserInfo(HttpContext, res);
            return true;
        }

        [HttpGet("exit")]
        public bool Exit()
        {
            sessionUtils.ClearString(HttpContext, "userId");
            sessionUtils.ClearString(HttpContext, "userName");
            return true;
        }

        [HttpPost("register")]
        public async Task<bool> Register([FromBody] UserInfo userInfo)
        {
            return await authService.Register(userInfo);
        }

        [HttpGet("checkUserNameExists/{name}")]
        public async Task<bool> CheckUserNameExists([FromRoute] string name)
        {
            return await authService.CheckUserNameExists(name);
        }

        [HttpGet("checkIfLoggedIn")]
        public UserInfo CheckIfLoggedIn()
        {
            return sessionUtils.GetCachedUserInfo(HttpContext);
        }

        [HttpGet("editUserName/{userName}")]
        public async Task<bool> EditUserName([FromRoute] string userName)
        {
            var res = sessionUtils.GetCachedUserInfo(HttpContext);
            if (res == null)
            {
                return false;
            }
            res.UserName = userName;
            return await authService.EditUserName(res);
        }
    }
}
