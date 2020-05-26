using System;
using System.Threading.Tasks;
using quiz_oj.Entities;
using quiz_oj.Entities.User;

namespace quiz_oj.Dao.interfaces
{
    public interface IUserInfoDao
    {
        Task<bool> AddUserInfo(UserInfo userInfo);
        Task<UserInfo> ValidateUserInfo(UserInfo userInfo);
        Task<bool> CheckUserNameExists(string name);

        Task<bool> EditUserName(string userId, string name);
    }
}