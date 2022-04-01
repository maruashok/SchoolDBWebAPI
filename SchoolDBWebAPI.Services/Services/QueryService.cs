using SchoolDBWebAPI.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public class QueryService<TEntity> : IQueryService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> repository;
        private readonly ILogger logger = Log.ForContext<TEntity>();

        public QueryService(IUnitOfWork _unitOfWork)
        {
            repository = _unitOfWork.GetRepository<TEntity>();
        }

        protected IRepository<TEntity> Repository
        {
            get { return repository; }
        }

        public TEntity GetByID(object id)
        {
            TEntity result = default;

            try
            {
                result = repository.GetByID(id);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }

        public Task<TEntity> GetByIDAsync(object id)
        {
            Task<TEntity> objResult = default;

            try
            {
                objResult = repository.GetByIDAsync(id);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return objResult;
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            int RowsAffected = -1;

            try
            {
                RowsAffected = repository.GetCount(filter);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return RowsAffected;
        }

        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            Task<int> result = default;

            try
            {
                result = repository.GetCountAsync(filter);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }

        public bool IsExists(Expression<Func<TEntity, bool>> filter = null)
        {
            bool result = default;

            try
            {
                result = repository.IsExists(filter);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }

        public Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            Task<bool> result = default;

            try
            {
                result = repository.GetExistsAsync(filter);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            TEntity result = default;

            try
            {
                result = repository.GetFirst(filter, includeProperties);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }

        public Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            Task<TEntity> result = default;

            try
            {
                result = repository.GetFirstAsync(filter, includeProperties);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }

        public IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            IEnumerable<TEntity> result = default;

            try
            {
                result = repository.GetWithRawSql(query, parameters);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            IEnumerable<TEntity> result = default;

            try
            {
                result = repository.Get(filter, orderBy, includeProperties, skip, take);
            }
            catch (Exception Ex)
            {
                logger.Information(Ex, Ex.Message);
            }

            return result;
        }
    }
}