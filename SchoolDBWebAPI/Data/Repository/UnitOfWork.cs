using Microsoft.EntityFrameworkCore.Storage;
using SchoolDBWebAPI.Data.Interfaces;
using SchoolDBWebAPI.DBModels;
using Serilog;
using System;
using System.Collections.Generic;

namespace SchoolDBWebAPI.Data.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private SchoolDBContext context = new();
        private IDbContextTransaction _transaction;
        private BaseRepository<QuizDetail> _quizDetailsRepository;
        private ILogger logger = Log.ForContext(typeof(UnitOfWork));

        public void BeginTransaction()
        {
            try
            {
                _transaction = context.Database.BeginTransaction();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
        }

        public UnitOfWork()
        {
            context = new SchoolDBContext();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            BaseRepository<T> repository = new BaseRepository<T>(context);

            if (repository != null)
            {
                return repository;
            }

            return null;
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
            int RowsAffected = -1;

            try
            {
                context.SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
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