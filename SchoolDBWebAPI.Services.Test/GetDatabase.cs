using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using SchoolDBWebAPI.Services.Repository;
using SchoolDBWebAPI.Services.Services;
using SchoolDBWebAPI.Services.SPHelper;
using System;
using System.IO;

namespace SchoolDBWebAPI.Services.Test
{
    public class GetDatabase : IDisposable
    {
        private bool disposedValue;
        public SchoolDBContext dBContext;

        public GetDatabase()
        {
            dBContext = new(new DbContextOptionsBuilder<SchoolDBContext>().UseInMemoryDatabase("SchoolDB").Options);
        }

        public bool FeedData()
        {
            bool Status = false;
            string JsonData = File.ReadAllText(@"JsonFiles\QuizController\AddQuiz.txt");

            if (!string.IsNullOrEmpty(JsonData))
            {
                QuizDetail quizDetail = JsonConvert.DeserializeObject<QuizDetail>(JsonData);

                if (dBContext != null)
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(dBContext);
                    IProcedureManager procedureManager = new ProcedureManager();
                    IQuizDetailService service = new QuizDetailService(unitOfWork, procedureManager);

                    Status = service.Insert(quizDetail).Id > 0;
                }
            }

            return Status;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dBContext.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}