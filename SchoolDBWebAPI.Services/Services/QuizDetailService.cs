using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Models.SP.Query;
using SchoolDBWebAPI.Services.SPHelper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SchoolDBWebAPI.Services.Services
{
    public interface IQuizDetailService : IQueryService<QuizDetail>, ICommandService<QuizDetail>
    {
        bool IsQuizExists(int QuizId);

        List<QuizDetail> SearchQuizByTitle(string QuizTitle);

        List<QuizDetail> AddQuiz(List<DBSQLParameter> SQLParams);

        List<QuizDetail> ListAllQuiz(Qry_SP_StudentMasterSelect model);
    }

    public class QuizDetailService : BaseService<QuizDetail>, IQuizDetailService
    {
        public IProcedureManager procedureManager;

        public QuizDetailService(IUnitOfWork uow, IProcedureManager _procedureManager) : base(uow)
        {
            procedureManager = _procedureManager;
        }

        public List<QuizDetail> AddQuiz(List<DBSQLParameter> SQLParams)
        {
            return procedureManager.ExecStoreProcedure<QuizDetail>("SP_QuizDetailInsert", SQLParams);
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
    }
}