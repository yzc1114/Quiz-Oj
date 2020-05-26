using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using quiz_oj.Dao.interfaces;
using quiz_oj.Entities.Quiz;

namespace quiz_oj.Dao.impls
{
    public class QuizDao : IQuizDao
    {
        private QOJDBContext dbContext;
        private DaoUtils daoUtils;
        
        public QuizDao(QOJDBContext qojdbContext, DaoUtils daoUtils)
        {
            dbContext = qojdbContext;
            this.daoUtils = daoUtils;
        }
        public async Task<List<QuizScore>> Rank(int page)
        {
            return await dbContext.QuizHighScores
                .OrderByDescending(s => s.HighScore)
                .Skip((page - 1) * 10)
                .Join(dbContext.UserInfos, s => s.UserId, u => u.Id, (s, u) => new QuizScore
                {
                    Score = s.HighScore,
                    UserId = u.Id,
                    UserName = u.UserName
                })
                .Take(10)
                .ToListAsync();
        }

        public async Task<QuizQuestion> GetQuestion(string questionId)
        {
            return await dbContext.QuizQuestions
                .Where(q => q.Id == questionId)
                .Include(q => q.OptionDTOList)
                .FirstAsync();
        }
        
        public async Task<QuizQuestion> GetQuestionThatSkip(int skip)
        {
            return await dbContext.QuizQuestions
                .Skip(skip)
                .Include(q => q.OptionDTOList)
                .FirstAsync();
        }

        public async Task<bool> Validate(string quizId, QuizSubmit quizSubmit)
        {
            var option = await dbContext.QuizOptions.Select(o => new QuizOption
            {
                Correct = o.Correct,
                Id = o.Id,
                QuizId = o.QuizId
            }).SingleOrDefaultAsync(o => o.QuizId == quizId && o.Id == quizSubmit.OptionId);
            return option.Correct.HasValue && option.Correct.Value;
        }

        public async Task<int> GetQuestionCount()
        {
            return await dbContext.QuizQuestions.CountAsync();
        }

        public async Task<int> GetHighScore(string userId)
        {
            var highScoreObj = await dbContext.QuizHighScores.SingleOrDefaultAsync(u => u.UserId == userId);
            return highScoreObj?.HighScore ?? 0;
        }

        public async Task<bool> SetHighScore(string userId, int score)
        {
            var highScore = await dbContext.QuizHighScores.SingleOrDefaultAsync(q => q.UserId == userId);
            if (highScore == null)
            {
                highScore = new QuizHighScore {HighScore = score, UserId = userId};
                await dbContext.QuizHighScores.AddAsync(highScore);
                await dbContext.SaveChangesAsync();
                return true;
            }

            if (score <= highScore.HighScore)
            {
                return true;
            }
            highScore.HighScore = score;
            dbContext.QuizHighScores.Update(highScore);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UploadQuiz(QuizQuestion question)
        {
            question.Id = daoUtils.GUID();
            foreach (var quizOption in question.OptionDTOList)
            {
                quizOption.QuizId = question.Id;
            }
            await using var tx = await dbContext.Database.BeginTransactionAsync();
            await dbContext.QuizQuestions.AddAsync(question);
            await dbContext.SaveChangesAsync();
            await tx.CommitAsync();

            return true;
        }
    }
}