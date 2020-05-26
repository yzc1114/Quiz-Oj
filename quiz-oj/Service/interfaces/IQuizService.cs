using System.Collections.Generic;
using System.Threading.Tasks;
using quiz_oj.Entities.Quiz;

namespace quiz_oj.Service.interfaces
{
    public interface IQuizService
    {
        Task<List<QuizScore>> Rank(int page);
        Task<QuizQuestion> GetQuestion(string questionId);
        Task<bool> Validate(string quizId, QuizSubmit quizSubmit);

        Task<QuizQuestion> GetRandomQuestionBaseOnPreviousQuestionList(List<string> previousQuestionList);

        Task<bool> SaveScore(string userId, int score);
    }
}