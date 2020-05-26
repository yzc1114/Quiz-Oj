using System.Threading.Tasks;
using quiz_oj.Entities.OJ;
using quiz_oj.Entities.Quiz;

namespace quiz_oj.Service.interfaces
{
    public interface IUploadService
    {
        Task<bool> UploadQuiz(QuizQuestion question);

        Task<bool> UploadOj(OjTestCaseSet ojTestCaseSet);

        Task<bool> FixOjOrderId();
    }
}