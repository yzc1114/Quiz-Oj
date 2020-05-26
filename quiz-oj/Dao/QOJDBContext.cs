using Microsoft.EntityFrameworkCore;
using quiz_oj.Entities;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.Quiz;
using quiz_oj.Entities.Review;
using quiz_oj.Entities.User;

namespace quiz_oj.Dao
{
    public class QOJDBContext : DbContext
    {
        public QOJDBContext(DbContextOptions<QOJDBContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuizOption>()
                .HasKey(c => new { c.QuizId, c.Id });
            modelBuilder.Entity<OjSuccess>()
                .HasKey(c => new { userId = c.UserId, ojQuestionId = c.OjQuestionId });
            //modelBuilder.Entity<QuizQuestion>().HasMany(b => b.OptionDTOList).WithOne();
            modelBuilder.Entity<QuizOption>()
                .HasOne<QuizQuestion>()
                .WithMany(b => b.OptionDTOList).HasForeignKey(p => p.QuizId);
            modelBuilder.Entity<OjSuccessCount>(c =>
            {
                c.HasOne(d => d.UserInfo).WithOne(u => u.OjSuccessCount).HasForeignKey<OjSuccessCount>(e => e.UserId);
            });
            modelBuilder.Entity<OjReview>(r =>
            {
                r.HasOne(d => d.UserInfo).WithMany().HasForeignKey(e => e.UserId);
            });
            modelBuilder.Entity<OjReview>().HasKey(p => new {p.OjId, p.CreateTime, p.UserId});
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<OjQuestion> OjQuestions { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizOption> QuizOptions { get; set; }
        public DbSet<OjSuccess> OjSuccesses { get; set; }
        public DbSet<OjSuccessCount> OjSuccessCounts { get; set; }
        public DbSet<QuizHighScore> QuizHighScores { get; set; }
        public DbSet<OjTestCaseTable> OjTestCaseTables { get; set; }
        public DbSet<OjReview> OjReviews { get; set; }
    }
}