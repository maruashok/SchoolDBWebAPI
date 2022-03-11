using SchoolDBWebAPI.Data.Interfaces;
using SchoolDBWebAPI.DBModels;
using System;

namespace SchoolDBWebAPI.Data.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private SchoolDBContext context = new();
        private BaseRepository<QuizDetail> _quizDetailsRepository;

        public IRepository<QuizDetail> QuizDetailRepository
        {
            get
            {
                if (_quizDetailsRepository == null)
                {
                    _quizDetailsRepository = new BaseRepository<QuizDetail>(context);
                }

                return _quizDetailsRepository;
            }
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}