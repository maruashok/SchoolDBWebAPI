using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public class BaseService<TEntity> : QueryService<TEntity>, IQueryService<TEntity>, IBaseService<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger = Log.ForContext<TEntity>();

        public BaseService(IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        protected void Commit()
        {
            try
            {
                unitOfWork.Commit();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
        }

        protected void Rollback()
        {
            try
            {
                unitOfWork.Rollback();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
        }

        protected void BeginTransaction()
        {
            try
            {
                unitOfWork.BeginTransaction();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }
        }

        protected int SaveChanges()
        {
            int RowsAffected = -1;

            try
            {
                RowsAffected = unitOfWork.SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        protected Task<int> SaveChangesAsync()
        {
            Task<int> RowsAffected = default;

            try
            {
                RowsAffected = unitOfWork.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual int Delete(TEntity entityToDelete)
        {
            int RowsAffected = default;

            try
            {
                Repository.Delete(entityToDelete);
                RowsAffected = SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual int DeleteById(object id)
        {
            int RowsAffected = default;

            try
            {
                Repository.DeleteById(id);
                RowsAffected = SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual async Task<int> DeleteByIdAsync(object id)
        {
            int RowsAffected = default;

            try
            {
                await Repository.DeleteByIdAsync(id);
                RowsAffected = await SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual int DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            int RowsAffected = default;

            try
            {
                Repository.DeleteRange(filter);
                RowsAffected = SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual async Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> filter)
        {
            int RowsAffected = default;

            try
            {
                Repository.DeleteRange(filter);
                RowsAffected = await SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual int Insert(TEntity entity)
        {
            int RowsAffected = default;

            try
            {
                Repository.Insert(entity);
                RowsAffected = SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            int RowsAffected = default;

            try
            {
                await Repository.InsertAsync(entity);
                RowsAffected = await SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual int InsertRange(List<TEntity> entities)
        {
            int RowsAffected = default;

            try
            {
                Repository.InsertRange(entities);
                RowsAffected = SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual async Task<int> InsertRangeAsync(List<TEntity> entities)
        {
            int RowsAffected = default;

            try
            {
                await Repository.InsertRangeAsync(entities);
                RowsAffected = await SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public virtual bool Update(TEntity entityToUpdate)
        {
            bool IsSaved = default;

            try
            {
                Repository.Update(entityToUpdate);
                IsSaved = SaveChanges() > 0;
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return IsSaved;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entityToUpdate)
        {
            bool IsSaved = default;

            try
            {
                Repository.Update(entityToUpdate);
                IsSaved = await SaveChangesAsync() > 0;
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, Ex.Message);
            }

            return IsSaved;
        }
    }
}