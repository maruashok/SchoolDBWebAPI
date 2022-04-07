using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entityToDelete);

        void DeleteById(object id);

        Task DeleteByIdAsync(object id);

        EntityEntry<TEntity> GetEntityEntry(TEntity entity);

        void SetEntityValues(TEntity entity, object Values);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);

        TEntity GetByID(object id);

        Task<TEntity> GetByIDAsync(object id);

        void DeleteRange(Expression<Func<TEntity, bool>> filter);

        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);

        bool IsExists(Expression<Func<TEntity, bool>> filter = null);

        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null);

        TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null);

        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);

        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        void InsertRange(List<TEntity> entities);

        Task InsertRangeAsync(List<TEntity> entities);

        void Update(TEntity entityToUpdate);
    }
}