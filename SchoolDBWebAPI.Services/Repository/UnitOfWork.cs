using Microsoft.EntityFrameworkCore.Storage;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly SchoolDBContext context;
        private IDbContextTransaction transaction;
        private readonly Dictionary<Type, object> repositories;
        private readonly ILogger logger = Log.ForContext(typeof(UnitOfWork));

        public void BeginTransaction()
        {
            try
            {
                transaction = context.Database.BeginTransaction();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
        }

        public UnitOfWork(SchoolDBContext dBContext)
        {
            context = dBContext;
            repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            IRepository<TEntity> repository = null;

            try
            {
                if (repositories.Keys.Contains(typeof(TEntity)))
                {
                    repository = (BaseRepository<TEntity>)(repositories[typeof(TEntity)] as IRepository<TEntity>);
                }
                else
                {
                    repository = new BaseRepository<TEntity>(context);

                    if (repository != null)
                    {
                        repositories.Add(typeof(TEntity), repository);
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return repository;
        }

        public int SaveChanges()
        {
            int RowsAffected = -1;

            try
            {
                RowsAffected = context.SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public Task<int> SaveChangesAsync()
        {
            Task<int> RowsAffected = default;

            try
            {
                RowsAffected = context.SaveChangesAsync();
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
                if (transaction != null)
                {
                    transaction.Commit();
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
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
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