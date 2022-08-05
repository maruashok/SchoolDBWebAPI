using Microsoft.Extensions.Logging;
using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Interfaces;
using System;

namespace SchoolDBWebAPI.DAL.Repository
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
            bool IsExists = false;

            try
            {
                IsExists = GetFirst(quiz => quiz.Id == QuizId) != null;
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex, Ex.Message);
            }

            return IsExists;
        }
    }
}