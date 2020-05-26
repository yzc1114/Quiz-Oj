using System;
using Microsoft.AspNetCore.Http;
using quiz_oj.Entities;
using quiz_oj.Entities.User;

namespace quiz_oj.Controllers
{
    public class SessionUtils
    {
        public UserInfo GetCachedUserInfo(HttpContext httpContext)
        {
            var userId = httpContext.Session.GetString("userId");
            var userName = httpContext.Session.GetString("userName");
            if (userId != null && userName != null)
            {
                return new UserInfo
                {
                    UserName = userName, Id = userId
                };
            }
            return null;
        }

        public void SetSessionUserInfo(HttpContext httpContext, UserInfo userInfo)
        {
            httpContext.Session.SetString("userId", userInfo.Id);
            httpContext.Session.SetString("userName", userInfo.UserName);
        }

        public string GetString(HttpContext httpContext, string key)
        {
            return httpContext.Session.GetString(key);
        }

        public void SetString(HttpContext httpContext, string key, string value)
        {
            httpContext.Session.SetString(key, value);
        }

        public void ClearString(HttpContext httpContext, string key)
        {
            httpContext.Session.Remove(key);
        }

        public Int32? GetInt(HttpContext httpContext, string key)
        {
            return httpContext.Session.GetInt32(key);
        }

        public void SetInt(HttpContext httpContext, string key, Int32 value)
        {
            httpContext.Session.SetInt32(key, value);
        }

        public void ClearInt(HttpContext httpContext, string key)
        {
            httpContext.Session.Remove(key);
        }
    }
}