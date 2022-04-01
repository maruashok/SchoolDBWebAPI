using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public class BaseService<TEntity> : QueryService<TEntity>, IQueryService<TEntity>, ICommandService<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<TEntity> repository;
        private readonly ILogger logger = Log.ForContext<TEntity>();

        public BaseService(IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            unitOfWork = _unitOfWork;
            repository = _unitOfWork.GetRepository<TEntity>();
        }

        public void Commit()
        {
            try
            {
                unitOfWork.Commit();
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public void Rollback()
        {
            try
            {
                unitOfWork.Rollback();
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public void BeginTransaction()
        {
            try
            {
                unitOfWork.BeginTransaction();
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public int SaveChanges()
        {
            int RowsAffected = -1;

            try
            {
                RowsAffected = unitOfWork.SaveChanges();
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public void Insert(TEntity entity)
        {
            try
            {
                Repository.Insert(entity);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public void Delete(TEntity quizDetail)
        {
            try
            {
                Repository.Delete(quizDetail);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public void DeleteById(object id)
        {
            try
            {
                Repository.DeleteById(id);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public async Task DeleteByIdAsync(object id)
        {
            try
            {
                await repository.DeleteByIdAsync(id);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                repository.DeleteRange(filter);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public async Task InsertAsync(TEntity entity)
        {
            try
            {
                await repository.InsertAsync(entity);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public void InsertRange(List<TEntity> entities)
        {
            try
            {
                repository.InsertRange(entities);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public async Task InsertRangeAsync(List<TEntity> entities)
        {
            try
            {
                await repository.InsertRangeAsync(entities);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }

        public void Update(TEntity entityToUpdate)
        {
            try
            {
                repository.Update(entityToUpdate);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }
        }
    }
}