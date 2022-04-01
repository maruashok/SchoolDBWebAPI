using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using SchoolDBWebAPI.Controllers;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Repository;
using SchoolDBWebAPI.Services.Services;
using SchoolDBWebAPI.Services.SPHelper;
using System.IO;
using Xunit;

namespace TestProject1
{
    public class QuizDetailsTest
    {
        [Fact]
        public void Test_GetQuizbyId()
        {
            IActionResult result;
            var options = new DbContextOptionsBuilder<SchoolDBContext>()
                        .UseInMemoryDatabase(databaseName: "SchoolDB").Options;

            using (SchoolDBContext dBContext = new(options))
            {
                IUnitOfWork unitOfWork = new UnitOfWork(dBContext);
                IProcedureManager procedureManager = new ProcedureManager();
                IQuizDetailService quizDetailService = new QuizDetailService(unitOfWork, procedureManager);
                var mock = new Mock<ILogger<QuizDetailController>>();
                ILogger<QuizDetailController> logger = mock.Object;

                QuizDetailController quizDetailController = new(quizDetailService, logger);

                result = quizDetailController.Get(2005);
            }

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Test_AddQuiz()
        {
            IActionResult result = null;
            var options = new DbContextOptionsBuilder<SchoolDBContext>()
                        .UseInMemoryDatabase(databaseName: "SchoolDB").Options;

            using (SchoolDBContext dBContext = new(options))
            {
                IUnitOfWork unitOfWork = new UnitOfWork(dBContext);

                string JsonData = File.ReadAllText(@"JsonFiles\QuizController\AddQuiz.txt");

                if (!string.IsNullOrEmpty(JsonData))
                {
                    QuizDetail quizDetail = JsonConvert.DeserializeObject<QuizDetail>(JsonData);

                    IProcedureManager procedureManager = new ProcedureManager();
                    IQuizDetailService quizDetailService = new QuizDetailService(unitOfWork, procedureManager);
                    var mock = new Mock<ILogger<QuizDetailController>>();
                    ILogger<QuizDetailController> logger = mock.Object;

                    QuizDetailController quizDetailController = new(quizDetailService, logger);

                    result = quizDetailController.Insert(quizDetail);
                    var expectedResult = quizDetailController.Get(2005);

                    Assert.Equal(expectedResult, result);
                }
            }

            Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}