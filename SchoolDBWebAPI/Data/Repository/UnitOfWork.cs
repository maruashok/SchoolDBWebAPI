using Microsoft.EntityFrameworkCore.Storage;
using SchoolDBWebAPI.Data.Interfaces;
using SchoolDBWebAPI.DBModels;
using System;

namespace SchoolDBWebAPI.Data.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private SchoolDBContext context = new();
        private IDbContextTransaction _transaction;
        private BaseRepository<QuizDetail> _quizDetailsRepository;

        public void BeginTransaction()
        {
            _transaction = context.Database.BeginTransaction();
        }

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

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
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