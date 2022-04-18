using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Repository;
using SchoolDBWebAPI.Services.Services;
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
                database.ClearDatabase();
                IQuizRepository repository = new QuizRepository(dBContext);
                IQuizDetailService service = new QuizDetailService(repository);

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
                IQuizRepository repository = new QuizRepository(dBContext);
                IQuizDetailService service = new QuizDetailService(repository);

                QuizDetail quizDetail = service.GetByID(2005);
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
                IQuizRepository repository = new QuizRepository(dBContext);
                IQuizDetailService service = new QuizDetailService(repository);

                QuizDetail quizDetail = service.GetFirst(data => data.Title.Contains(quizDetailORG.Title));

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
                IQuizRepository repository = new QuizRepository(dBContext);
                IQuizDetailService service = new QuizDetailService(repository);

                if (service.DeleteByID(quizDetailORG.Id))
                {
                    quizDetail = service.GetByID(quizDetailORG.Id);
                }

                Assert.Null(quizDetail);
            }
        }
    }
}