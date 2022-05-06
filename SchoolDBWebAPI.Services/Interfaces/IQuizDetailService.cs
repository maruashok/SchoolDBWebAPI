using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Models.SP.Query;
using SchoolDBWebAPI.Services.SPHelper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IQuizDetailService
    {
        bool DeleteByID(int QuizId);

        bool IsQuizExists(int QuizId);

        QuizDetail GetByID(int QuizId);

        List<QuizDetail> GetAllQuiz();

        QuizDetail GetQuizById(int QuizId);

        QuizDetail SearchQuiz(string QuizTitle);

        QuizDetail Insert(QuizDetail quizDetail);

        Task<bool> UpdateAsync(QuizDetail quizDetail);

        Task<QuizDetail> QuizWithQuesAsync(int QuizId);

        QuizDetail AddQuiz(List<DBSQLParameter> SQLParams);

        List<QuizDetail> SearchQuizByTitle(string QuizTitle);

        int DeleteRange(Expression<Func<QuizDetail, bool>> filter);

        List<QuizDetail> ListAllQuiz(Qry_SP_StudentMasterSelect model);
    }
}