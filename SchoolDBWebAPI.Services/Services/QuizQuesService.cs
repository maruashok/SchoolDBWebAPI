using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Repository;
using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;

namespace SchoolDBWebAPI.Services.Services
{
    public interface IQuizQuesService
    {
        bool AddQues(QuizQuestion question);
    }

    public class QuizQuesService : IQuizQuesService
    {
        private readonly ILogger logger;
        private readonly IQuizQuesRepository Repository;
        private readonly IQuizDetailService QuizService;

        public QuizQuesService(IQuizQuesRepository _repository, IQuizRepository quizRepository)
        {
            Repository = _repository;
            logger = Log.ForContext<QuizDetailService>();
            QuizService = new QuizDetailService(quizRepository);
        }

        public bool AddQues(QuizQuestion question)
        {
            bool IsAdded = false;

            try
            {
                QuizDetail quizDetail = QuizService.GetByID(question.QuizId);

                if (quizDetail != null)
                {
                    Repository.Insert(question);
                    IsAdded = Repository.SaveChanges() > 0;
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return IsAdded;
        }
    }
}