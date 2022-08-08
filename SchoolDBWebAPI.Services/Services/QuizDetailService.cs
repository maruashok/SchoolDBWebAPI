using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Extensions;
using SchoolDBWebAPI.DAL.Models.SP.Query;
using SchoolDBWebAPI.DAL.Repository;
using SchoolDBWebAPI.DAL.SPHelper;
using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public class QuizDetailService : IQuizDetailService
    {
        private readonly IQuizRepository Repository;

        public QuizDetailService(IQuizRepository _repository)
        {
            Repository = _repository;
        }

        public QuizDetail AddQuiz(List<DBSQLParameter> SQLParams)
        {
            return Repository.AddQuiz(SQLParams);
        }

        public async Task<bool> UpdateAsync(QuizDetail model)
        {
            return await Repository.UpdateAsync(model);
        }

        public bool IsQuizExists(int QuizId)
        {
            return Repository.IsExists(quiz => quiz.Id == QuizId);
        }

        public List<QuizDetail> ListAllQuiz(Qry_SP_StudentMasterSelect model)
        {
            return Repository.GetAll(model);
        }

        public List<QuizDetail> SearchQuizByTitle(string QuizTitle)
        {
            return Repository.SearchQuizByTitle(QuizTitle);
        }

        public QuizDetail SearchQuiz(string QuizTitle)
        {
            return Repository.GetFirst(quiz => quiz.Title.Contains(QuizTitle));
        }

        public async Task<QuizDetail> QuizWithQuesAsync(int QuizId)
        {
            return await Repository.QuizWithQuesAsync(QuizId); ;
        }

        public QuizDetail QuizWithQues(int QuizId)
        {
            return Repository.QuizWithQues(QuizId);
        }

        public QuizDetail GetByID(int QuizId)
        {
            return Repository.GetByID(QuizId);
        }

        public bool Insert(QuizDetail quizDetail)
        {
            return Repository.Insert(quizDetail);
        }

        public bool DeleteByID(int QuizId)
        {
            return Repository.DeleteByID(QuizId);
        }

        public List<QuizDetail> GetAllQuiz()
        {
            return Repository.GetAll().ToList();
        }
    }
}