using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Models.SP.Query;
using SchoolDBWebAPI.Services.Repository;
using SchoolDBWebAPI.Services.SPHelper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public class QuizDetailService : IQuizDetailService
    {
        private readonly ILogger logger;
        private readonly IQuizRepository Repository;

        public QuizDetailService(IQuizRepository _repository)
        {
            Repository = _repository;
            logger = Log.ForContext<QuizDetailService>();
        }

        public QuizDetail AddQuiz(List<DBSQLParameter> SQLParams)
        {
            return Repository.ExecStoreProcedure<QuizDetail>("SP_QuizDetailInsert", SQLParams).FirstOrDefault();
        }

        public async Task<bool> UpdateAsync(QuizDetail model)
        {
            bool isUpdated = default;

            try
            {
                QuizDetail quizDetail = await Repository.GetFirstAsync(quiz => quiz.Id == model.Id, includeProperties: "QuizQuestions");

                if (quizDetail != null)
                {
                    quizDetail.QuizQuestions.Clear();
                    Repository.SetEntityValues(quizDetail, model);
                    quizDetail.QuizQuestions = model.QuizQuestions;
                    Repository.Update(quizDetail);
                    isUpdated = await Repository.SaveChangesAsync() > 0;
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return isUpdated;
        }

        public bool IsQuizExists(int QuizId)
        {
            return Repository.IsExists(quiz => quiz.Id == QuizId);
        }

        public List<QuizDetail> ListAllQuiz(Qry_SP_StudentMasterSelect model)
        {
            return Repository.ExecStoreProcedure<QuizDetail>("SP_StudentMasterSelect", model);
        }

        public List<QuizDetail> SearchQuizByTitle(string QuizTitle)
        {
            return Repository.ExecuteSelect<QuizDetail>($@"Select * from QuizDetail where Title like '%' + @Qry +'%'", new SqlParameter("@Qry", QuizTitle));
        }

        public QuizDetail SearchQuiz(string QuizTitle)
        {
            return Repository.GetFirst(quiz => quiz.Title.Contains(QuizTitle));
        }

        public async Task<QuizDetail> QuizWithQuesAsync(int QuizId)
        {
            QuizDetail quizDetail = default;

            try
            {
                quizDetail = await Repository.GetFirstAsync(quiz => quiz.Id == QuizId, includeProperties: "QuizQuestions");
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return quizDetail;
        }

        public QuizDetail GetByID(int QuizId)
        {
            QuizDetail quizDetail = default;
            try
            {
                quizDetail = Repository.GetByID(QuizId);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
            return quizDetail;
        }

        public QuizDetail Insert(QuizDetail quizDetail)
        {
            Repository.Insert(quizDetail);
            Repository.SaveChanges();
            return quizDetail;
        }

        public QuizDetail GetFirst(Expression<Func<QuizDetail, bool>> filter = null, string includeProperties = null)
        {
            return Repository.GetFirst(filter, includeProperties);
        }

        public bool DeleteByID(int QuizId)
        {
            Repository.DeleteById(QuizId);
            return Repository.SaveChanges() > 0;
        }

        public int DeleteRange(Expression<Func<QuizDetail, bool>> filter)
        {
            Repository.DeleteRange(filter);
            return Repository.SaveChanges();
        }

        public List<QuizDetail> GetAllQuiz()
        {
            return Repository.Get().ToList();
        }
    }
}