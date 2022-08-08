using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Models.SP.Query;
using SchoolDBWebAPI.DAL.SPHelper;
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

        QuizDetail SearchQuiz(string QuizTitle);

        bool Insert(QuizDetail quizDetail);

        public QuizDetail QuizWithQues(int QuizId);

        Task<bool> UpdateAsync(QuizDetail quizDetail);

        Task<QuizDetail> QuizWithQuesAsync(int QuizId);

        QuizDetail AddQuiz(List<DBSQLParameter> SQLParams);

        List<QuizDetail> SearchQuizByTitle(string QuizTitle);

        List<QuizDetail> ListAllQuiz(Qry_SP_StudentMasterSelect model);
    }
}