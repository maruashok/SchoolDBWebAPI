using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Models.SP.Query;
using SchoolDBWebAPI.DAL.SPHelper;

namespace SchoolDBWebAPI.DAL.Interfaces
{
    public interface IQuizRepository : IBaseRepository<QuizDetail>
    {
        bool DeleteByID(int QuizId);

        bool IsQuizExists(int QuizId);

        bool Insert(QuizDetail quizDetail);

        QuizDetail QuizWithQues(int QuizId);

        Task<QuizDetail> QuizWithQuesAsync(int QuizId);

        Task<bool> UpdateAsync(QuizDetail model);

        QuizDetail AddQuiz(List<DBSQLParameter> SQLParams);

        List<QuizDetail> SearchQuizByTitle(string QuizTitle);

        List<QuizDetail> GetAll(Qry_SP_StudentMasterSelect model);
    }
}