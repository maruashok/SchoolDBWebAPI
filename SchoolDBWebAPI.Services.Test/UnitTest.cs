using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Repository;
using SchoolDBWebAPI.Services.Services;
using SchoolDBWebAPI.Services.SPHelper;
using Xunit;

namespace SchoolDBWebAPI.Services.Test
{
    public class UnitTest
    {
        private GetDatabase database;
        private SchoolDBContext dBContext;

        public UnitTest()
        {
            database = new GetDatabase();
            dBContext = database.dBContext;
        }

        [Fact]
        public void Test_AddQuiz()
        {
            QuizDetail quizDetail = default;
            QuizDetail quizDetailORG = FeedDBData.GetQuizDetail();

            if (dBContext != null)
            {
                IUnitOfWork unitOfWork = new UnitOfWork(dBContext);
                IProcedureManager procedureManager = new ProcedureManager();
                IQuizDetailService service = new QuizDetailService(unitOfWork, procedureManager);

                if (service.Insert(quizDetailORG).Id > 0)
                {
                    quizDetail = service.GetByID(quizDetailORG.Id);
                }

                Assert.Equal(quizDetailORG.Id, quizDetail.Id);
            }
        }

        [Fact]
        public void Test_GetQuiz()
        {
            database.FeedData();
            QuizDetail quizDetailORG = FeedDBData.GetQuizDetail();

            if (dBContext != null)
            {
                IUnitOfWork unitOfWork = new UnitOfWork(dBContext);
                IProcedureManager procedureManager = new ProcedureManager();
                IQuizDetailService quizDetailService = new QuizDetailService(unitOfWork, procedureManager);

                QuizDetail quizDetail = quizDetailService.GetByID(2005);

                Assert.Equal(quizDetailORG.Id, quizDetail.Id);
            }
        }

        [Fact]
        public void Test_SearchQuiz()
        {
            database.FeedData();
            QuizDetail quizDetailORG = FeedDBData.GetQuizDetail();

            if (dBContext != null)
            {
                IUnitOfWork unitOfWork = new UnitOfWork(dBContext);
                IProcedureManager procedureManager = new ProcedureManager();
                IQuizDetailService quizDetailService = new QuizDetailService(unitOfWork, procedureManager);

                QuizDetail quizDetail = quizDetailService.GetFirst(data => data.Title.Contains(quizDetailORG.Title));

                if (quizDetail != null)
                {
                    Assert.Equal(quizDetailORG.Title, quizDetail.Title);
                }
            }
        }

        [Fact]
        public void Test_DeleteQuiz()
        {
            database.FeedData();
            QuizDetail quizDetail = default;
            QuizDetail quizDetailORG = FeedDBData.GetQuizDetail();

            if (dBContext != null)
            {
                IUnitOfWork unitOfWork = new UnitOfWork(dBContext);
                IProcedureManager procedureManager = new ProcedureManager();
                IQuizDetailService service = new QuizDetailService(unitOfWork, procedureManager);

                if (service.Delete(quizDetailORG) > 0)
                {
                    quizDetail = service.GetByID(quizDetailORG.Id);
                }

                Assert.Null(quizDetail);
            }
        }
    }
}