using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchoolDBWebAPI.DAL.DBModels;
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

        public void ClearDatabase()
        {
            try
            {
                dBContext.Database.EnsureDeleted();
                dBContext.Database.EnsureCreated();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        public bool FeedData()
        {
            bool Status = false;
            string JsonData = File.ReadAllText(@"JsonFiles\QuizController\AddQuiz.txt");

            if (!string.IsNullOrEmpty(JsonData))
            {
                QuizDetail quizDetail = JsonConvert.DeserializeObject<QuizDetail>(JsonData);

                if (quizDetail != null)
                {
                    ClearDatabase();
                    dBContext.QuizDetails.Add(quizDetail);
                    Status = dBContext.SaveChanges() > 0;
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