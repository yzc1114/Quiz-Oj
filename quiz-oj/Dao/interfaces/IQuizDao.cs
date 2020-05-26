using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Entities.Quiz;

namespace quiz_oj.Dao.interfaces
{
    public interface IQuizDao
    {
        Task<List<QuizScore>> Rank(int page);
        Task<QuizQuestion> GetQuestion(string questionId);
        Task<QuizQuestion> GetQuestionThatSkip(int skip);
        Task<bool> Validate(string quizId, QuizSubmit quizSubmit);

        Task<int> GetQuestionCount();

        Task<int> GetHighScore(string userId);

        Task<bool> SetHighScore(string userId, int score);

        Task<bool> UploadQuiz(QuizQuestion question);
    }
}