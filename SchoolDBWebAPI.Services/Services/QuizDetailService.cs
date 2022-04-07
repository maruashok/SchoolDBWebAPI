using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Models.SP.Query;
using SchoolDBWebAPI.Services.SPHelper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public interface IQuizDetailService : IQueryService<QuizDetail>, IBaseService<QuizDetail>
    {
        bool IsQuizExists(int QuizId);

        Task<QuizDetail> QuizWithQuesAsync(int QuizId);

        QuizDetail AddQuiz(List<DBSQLParameter> SQLParams);

        List<QuizDetail> SearchQuizByTitle(string QuizTitle);

        List<QuizDetail> ListAllQuiz(Qry_SP_StudentMasterSelect model);
    }

    public class QuizDetailService : BaseService<QuizDetail>, IQuizDetailService
    {
        public IProcedureManager procedureManager;
        private readonly ILogger logger = Log.ForContext<QuizDetailService>();

        public QuizDetailService(IUnitOfWork uow, IProcedureManager _procedureManager) : base(uow)
        {
            procedureManager = _procedureManager;
        }

        public QuizDetail AddQuiz(List<DBSQLParameter> SQLParams)
        {
            return procedureManager.ExecStoreProcedure<QuizDetail>("SP_QuizDetailInsert", SQLParams).FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(QuizDetail model)
        {
            bool isUpdated = default;

            try
            {
                QuizDetail quizDetail = await GetFirstAsync(quiz => quiz.Id == model.Id, includeProperties: "QuizQuestions");

                if (quizDetail != null)
                {
                    quizDetail.QuizQuestions.Clear();
                    Repository.SetEntityValues(quizDetail, model);
                    quizDetail.QuizQuestions = model.QuizQuestions;
                    isUpdated = await base.UpdateAsync(quizDetail);
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
            return procedureManager.ExecStoreProcedure<QuizDetail>("SP_StudentMasterSelect", model);
        }

        public List<QuizDetail> SearchQuizByTitle(string QuizTitle)
        {
            return procedureManager.ExecuteSelect<QuizDetail>($@"Select * from QuizDetail where Title like '%' + @Qry +'%'", new SqlParameter("@Qry", QuizTitle));
        }

        public async Task<QuizDetail> QuizWithQuesAsync(int QuizId)
        {
            QuizDetail quizDetail = default;

            try
            {
                quizDetail = await GetFirstAsync(quiz => quiz.Id == QuizId, includeProperties: "QuizQuestions");
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return quizDetail;
        }
    }
}