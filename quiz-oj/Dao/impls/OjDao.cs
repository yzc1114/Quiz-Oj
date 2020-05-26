using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.User;

namespace quiz_oj.Dao.impls
{
    public class OjDao : IOjDao
    {
        private QOJDBContext dbContext;
        private DaoUtils daoUtils;
        
        public OjDao(QOJDBContext qojdbContext, DaoUtils daoUtils)
        {
            dbContext = qojdbContext;
            this.daoUtils = daoUtils;
        }
        
        public async Task<List<OjPassCount>> GetRank(int page)
        {
            var list = await dbContext.OjSuccessCounts
                .OrderByDescending(c => c.Count)
                .Skip((page - 1) * 10)
                .Take(10)
                .Include(c => c.UserInfo)
                .Select(c => new OjPassCount
                {
                    PassCount = c.Count,
                    UserId = c.UserId,
                    UserName = c.UserInfo.UserName
                })
                // .Join(dbContext.UserInfos, c => c.UserId, u => u.Id, (c, u) => new OjPassCount
                // {
                //     PassCount = c.Count,
                //     UserId = u.Id,
                //     UserName = u.UserName
                // })
                .ToListAsync();
            return list;
        }

        public async Task<List<OjQuestion>> ListQuestionSummary(string userId, int page)
        {
            var list = await dbContext.OjQuestions
                .OrderBy(q => q.CreateTime)
                .Skip((page - 1) * 10)
                .Take(10)
                .Select(q => new OjQuestion
                {
                    Id = q.Id,
                    Title = q.Title,
                    CreateTime = q.CreateTime,
                    Difficulty = q.Difficulty,
                    OrderId = q.OrderId
                })
                .ToListAsync();
            if (userId == null)
            {
                return list;
            }

            var successes = await (from s in dbContext.OjSuccesses
                where s.UserId == userId && (from q in list select q.Id).Contains(s.OjQuestionId)
                select s.OjQuestionId).ToListAsync();
            var ojIdMap = new Hashtable();
            list.ForEach(q => ojIdMap.Add(q.Id, q));
            foreach (var q in successes.Select(ojSuccess => ojIdMap[ojSuccess] as OjQuestion))
            {
                if (q != null)
                {
                    q.Passed = true;
                }
            }
            return list;
        }

        public async Task<OjQuestion> GetQuestion(string id)
        {
            return await dbContext.OjQuestions.SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<string> UploadOj(OjQuestion question, OjTestCaseTable ojTestCaseTable)
        {
            question.Id = daoUtils.GUID();
            ojTestCaseTable.OjId = question.Id;
            await using var tx = await dbContext.Database.BeginTransactionAsync();
            var count = await dbContext.OjQuestions.CountAsync();
            question.OrderId = count + 1;
            await dbContext.OjQuestions.AddAsync(question);
            await dbContext.SaveChangesAsync();
            ojTestCaseTable.OjId = question.Id;
            await dbContext.OjTestCaseTables.AddAsync(ojTestCaseTable);
            await dbContext.SaveChangesAsync();
            await tx.CommitAsync();
            return question.Id;
        }

        public async Task<bool> FixOjOrderId()
        {
            var list = await dbContext.OjQuestions.ToListAsync();
            list.Sort((o1, o2) => o1.CreateTime.CompareTo(o2.CreateTime));
            for (var i = 0; i < list.Count; i++)
            {
                list[i].OrderId = i + 1;
            }
            dbContext.OjQuestions.UpdateRange(list);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveSuccess(string userId, string ojQuestionId)
        {
            await using var tx = await dbContext.Database.BeginTransactionAsync();
            var ojSuccess = await dbContext.OjSuccesses.FirstOrDefaultAsync(o => o.UserId == userId && o.OjQuestionId == ojQuestionId);
            if (ojSuccess == null)
            {
                await dbContext.OjSuccesses.AddAsync(new OjSuccess {UserId = userId, OjQuestionId = ojQuestionId});
                var ojSuccessCount = await dbContext.OjSuccessCounts.SingleOrDefaultAsync(c => c.UserId == userId);
                if (ojSuccessCount == null)
                {
                    ojSuccessCount = new OjSuccessCount {UserId = userId, Count = 1};
                    await dbContext.OjSuccessCounts.AddAsync(ojSuccessCount);
                }
                else
                {
                    ojSuccessCount.Count++;
                    dbContext.OjSuccessCounts.Update(ojSuccessCount);
                }
                await dbContext.SaveChangesAsync();
            }
            await tx.CommitAsync();
            return true;
        }

        public async Task<string> GetTestCase(string ojQuestionId)
        {
            var t = await dbContext.OjTestCaseTables.SingleOrDefaultAsync(q => q.OjId == ojQuestionId);
            return t?.TestCaseSetJson;
        }
    }
    
    
}