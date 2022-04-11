using Moq;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Services;
using Xunit;

namespace SchoolDBWebAPI.Services.Test
{
    public class MockTest
    {
        [Fact]
        public void MockTestGet()
        {
            Mock<IRepository<QuizDetail>> repoMock = new();
            QuizDetail quizDetail = FeedDBData.GetQuizDetail();
            Mock<IUnitOfWork> mockUoW = new Mock<IUnitOfWork>();
            repoMock.Setup(mock => mock.GetByID(2005)).Returns(quizDetail);
            Mock<IProcedureManager> procMock = new Mock<IProcedureManager>();
            mockUoW.Setup(m => m.GetRepository<QuizDetail>()).Returns(repoMock.Object);

            IQuizDetailService serviceMock = new QuizDetailService(mockUoW.Object, procMock.Object);

            if (serviceMock != null)
            {
                Assert.Equal(quizDetail.Id, serviceMock.GetByID(2005).Id);
            }
        }
    }
}