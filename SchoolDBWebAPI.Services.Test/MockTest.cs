using MockQueryable.Moq;
using Moq;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Repository;
using SchoolDBWebAPI.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace SchoolDBWebAPI.Services.Test
{
    public class MockTest
    {
        private QuizDetail quizDetail;
        private List<QuizDetail> quizDetails;
        private Mock<IQuizRepository> repoMock;

        public MockTest()
        {
            this.repoMock = new();
            this.quizDetail = FeedDBData.GetQuizDetail();
            this.quizDetails = FeedDBData.GetQuizDetails();
        }

        [Fact]
        public void Test_GetByID()
        {
            repoMock.Setup(mock => mock.GetByID(1)).Returns(quizDetail);

            IQuizDetailService service = new QuizDetailService(repoMock.Object);

            if (service != null)
            {
                Assert.Equal(quizDetail.Id, service.GetByID(1).Id);
            }
        }

        [Fact]
        public void Test_GetFirst()
        {
            repoMock.Setup(mock => mock.GetByID(1)).Returns(quizDetail);
            repoMock.Setup(quiz => quiz.GetFirst(quiz => quiz.Title.Contains(quizDetail.Title) && quiz.Id != quizDetail.Id, It.IsAny<string>())).Returns(quizDetail);

            IQuizDetailService service = new QuizDetailService(repoMock.Object);

            if (service != null)
            {
                QuizDetail quiz = service.GetQuizById(1);
                Assert.NotNull(quiz);
                Assert.Equal(quizDetail.Id, quiz.Id);
            }
        }

        [Fact]
        public void Test_SearchQuiz()
        {
            var queryMock = quizDetails.BuildMock();
            repoMock.Setup(mock => mock.GetQueryable()).Returns(queryMock);
            IQuizDetailService service = new QuizDetailService(repoMock.Object);
            repoMock.Setup(quiz => quiz.GetFirst(It.IsAny<Expression<Func<QuizDetail, bool>>>(), It.IsAny<string>())).Returns(quizDetail);

            if (service != null)
            {
                QuizDetail quiz = service.SearchQuiz(quizDetail.Title);

                Assert.NotNull(quiz);
                Assert.Equal(quizDetail.Title, quiz.Title);
                repoMock.Verify(quiz => quiz.GetFirst(It.IsAny<Expression<Func<QuizDetail, bool>>>(), It.IsAny<string>()));
            }
        }

        [Fact]
        public void Test_GetAll()
        {
            repoMock.Setup(mock => mock.Get(null, null, null, null, null)).Returns(quizDetails);
            IQuizDetailService service = new QuizDetailService(repoMock.Object);

            if (service != null)
            {
                List<QuizDetail> list = service.GetAllQuiz();

                Assert.NotNull(list);
                repoMock.Verify(quiz => quiz.Get(null, null, null, null, null));
                Assert.Equal(quizDetails.Count, list.Count);
            }
        }

        [Fact]
        public void Test_Add()
        {
            IQuizDetailService service = new QuizDetailService(repoMock.Object);

            if (service != null)
            {
                service.Insert(quizDetail);
                repoMock.Verify(r => r.Insert(It.IsAny<QuizDetail>()));
                repoMock.Verify(r => r.SaveChanges());
            }
        }

        [Fact]
        public void Test_AddQues()
        {
            Mock<IQuizQuesRepository> repoQuesMock = new();
            Mock<IQuizRepository> repoQuizMock = new();
            //Mock<IQuizDetailService> quizServiceMock = new();
            repoQuizMock.Setup(mock => mock.GetByID(quizDetail.Id)).Returns(quizDetail);

            IQuizQuesService service = new QuizQuesService(repoQuesMock.Object, repoQuizMock.Object);

            if (service != null)
            {
                QuizQuestion question = new QuizQuestion()
                {
                    QuizId = 11,
                    Description = "Test"
                };

                service.AddQues(question);
                repoQuesMock.Verify(r => r.Insert(It.IsAny<QuizQuestion>()));
                repoQuesMock.Verify(r => r.SaveChanges());
            }
        }

        [Fact]
        public void Test_Delete()
        {
            IQuizDetailService service = new QuizDetailService(repoMock.Object);

            if (service != null)
            {
                service.DeleteByID(quizDetail.Id);
                repoMock.Verify(r => r.DeleteById(It.IsAny<int>()));
                repoMock.Verify(r => r.SaveChanges(), Times.Exactly(1));
            }
        }

        [Fact]
        public void Test_DeleteRange()
        {
            var queryMock = quizDetails.BuildMock();
            repoMock.Setup(mock => mock.GetQueryable()).Returns(queryMock);
            IQuizDetailService service = new QuizDetailService(repoMock.Object);
            repoMock.Setup(quiz => quiz.DeleteRange(It.IsAny<Expression<Func<QuizDetail, bool>>>()));

            if (service != null)
            {
                service.DeleteRange(quiz => quiz.Id > 0);

                repoMock.Verify(r => r.SaveChanges());
                repoMock.Verify(quiz => quiz.DeleteRange(It.IsAny<Expression<Func<QuizDetail, bool>>>()));
            }
        }
    }
}