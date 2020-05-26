using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities;
using quiz_oj.Entities.User;
using quiz_oj.Service.interfaces;

namespace quiz_oj.Service.impls
{
    public class AuthService : IAuthService
    {
        private IUserInfoDao userInfoDao;
        
        public AuthService(IUserInfoDao userInfoDao)
        {
            this.userInfoDao = userInfoDao;
        }
        
        public async Task<bool> Register(UserInfo userInfo)
        {
            return await userInfoDao.AddUserInfo(userInfo);
        }

        public async Task<UserInfo> ValidateUserInfo(UserInfo userInfo)
        {
            return await userInfoDao.ValidateUserInfo(userInfo);
        }

        public async Task<bool> CheckUserNameExists(string name)
        {
            return await userInfoDao.CheckUserNameExists(name);
        }

        public Task<bool> EditUserName(UserInfo newUserInfo)
        {
            return userInfoDao.EditUserName(newUserInfo.Id, newUserInfo.UserName);
        }
    }
}