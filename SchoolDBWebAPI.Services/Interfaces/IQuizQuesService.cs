using SchoolDBWebAPI.DAL.DBModels;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IQuizQuesService
    {
        bool AddQues(QuizQuestion question);
    }
}