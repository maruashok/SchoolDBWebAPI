using SchoolDBWebAPI.DAL.DBModels;

namespace SchoolDBWebAPI.DAL.Interfaces
{
    public interface IQuizRepository
    {
        bool DeleteByID(int QuizId);
        bool IsQuizExists(int QuizId);
        Task<QuizDetail> QuizWithQuesAsync(int QuizId);
        Task<bool> UpdateAsync(QuizDetail model);
    }
}