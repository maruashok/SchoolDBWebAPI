using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolDBWebAPI.Services.Services
{
    public interface IQuizResultService : IQueryService<QuizResponse>
    {
        bool IsQuizResponseExists(int ResponseId);

        List<QuizResponse> GetQuizResponses(int QuizId);
    }

    public class QuizResultService : QueryService<QuizResponse>, IQuizResultService
    {
        public IProcedureManager procedureManager;
        private readonly ILogger logger = Log.ForContext(typeof(QuizResultService));

        public QuizResultService(IUnitOfWork uow, IProcedureManager _procedureManager) : base(uow)
        {
            procedureManager = _procedureManager;
        }

        public List<QuizResponse> GetQuizResponses(int QuizId)
        {
            List<QuizResponse> result = default;

            try
            {
                result = Repository.Get(data => data.QuizId == QuizId).ToList();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return result;
        }

        public bool IsQuizResponseExists(int ResponseId)
        {
            try
            {
                return Repository.IsExists(quiz => quiz.Id == ResponseId);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
                return false;
            }
        }
    }
}