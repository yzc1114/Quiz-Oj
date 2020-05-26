using System;
using System.Threading.Tasks;
using quiz_oj.Entities;
using quiz_oj.Entities.User;

namespace quiz_oj.Service.interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(UserInfo userInfo);
        Task<UserInfo> ValidateUserInfo(UserInfo userInfo);
        Task<bool> CheckUserNameExists(string name);
        Task<bool> EditUserName(UserInfo newUserInfo);
    }
}