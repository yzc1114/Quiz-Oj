using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities;
using quiz_oj.Entities.User;

namespace quiz_oj.Dao.impls
{
    public class UserInfoDao : IUserInfoDao
    {
        private QOJDBContext dbContext;
        private DaoUtils daoUtils;
        
        public UserInfoDao(QOJDBContext qojdbContext, DaoUtils daoUtils)
        {
            dbContext = qojdbContext;
            this.daoUtils = daoUtils;
        }
        
        public async Task<bool> AddUserInfo(UserInfo userInfo)
        {
            userInfo.Id = daoUtils.GUID();
            await dbContext.UserInfos.AddAsync(userInfo);
            return await dbContext.SaveChangesAsync() == 1;
        }

        public async Task<UserInfo> ValidateUserInfo(UserInfo userInfo)
        {
            return await dbContext.UserInfos.SingleOrDefaultAsync(p => p.UserName == userInfo.UserName);
        }

        public async Task<bool> CheckUserNameExists(string name)
        {
            await dbContext.UserInfos.CountAsync(p => p.UserName == name);
            var res = await dbContext.UserInfos.SingleOrDefaultAsync(p => p.UserName == name);
            return res != null;
        }

        public async Task<bool> EditUserName(string userId, string name)
        {
            var userInfo = new UserInfo {Id = userId, UserName = name};
            dbContext.Attach(userInfo);
            dbContext.Entry(userInfo).Property(p => p.UserName).IsModified = true;
            var c = await dbContext.SaveChangesAsync();
            return c == 1;
        }
    }
}