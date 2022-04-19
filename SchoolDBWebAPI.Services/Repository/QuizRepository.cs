using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.Services.DBModels;
using System;

namespace SchoolDBWebAPI.Services.Repository
{
    public interface IQuizRepository : IRepository<QuizDetail>
    {
        bool IsQuizExists(int QuizId);
    }

    public class QuizRepository : BaseRepository<QuizDetail>, IQuizRepository
    {
        private readonly ILogger logger;

        public QuizRepository(ILogger<QuizRepository> _logger, SchoolDBContext dBContext) : base(dBContext)
        {
            logger = _logger;
        }

        public bool IsQuizExists(int QuizId)
        {
            try
            {
                return GetFirst(quiz => quiz.Id == QuizId) != null;
            }
            catch (Exception Ex)
            {
                logger.LogError
                throw;
            }
            logger.LogInformation("Get Quiz By Id :- ", QuizId);
        }
    }
}