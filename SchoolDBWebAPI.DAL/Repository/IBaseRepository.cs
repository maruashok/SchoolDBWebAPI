using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolDBWebAPI.DAL.Interfaces;
using System.Linq.Expressions;

namespace SchoolDBWebAPI.DAL.Repository
{
    public interface IBaseRepository<TEntity> : IProcedureManager where TEntity : class
    {
        void BeginTransaction();

        void Commit();

        void Delete(TEntity entityToDelete);

        void DeleteById(object id);

        Task DeleteByIdAsync(object id);

        void DeleteRange(Expression<Func<TEntity, bool>> filter);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, bool>>[] includes);

        IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        TEntity GetByID(object id);

        Task<TEntity> GetByIDAsync(object id);

        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);

        EntityEntry<TEntity> GetEntityEntry(TEntity entity);

        TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null);

        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);

        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        void InsertRange(List<TEntity> entities);

        Task InsertRangeAsync(List<TEntity> entities);

        bool IsExists(Expression<Func<TEntity, bool>> filter = null);

        Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> filter = null);

        void Rollback();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void SetEntityValues(TEntity entity, object Values);

        void Update(TEntity entityToUpdate);

        void Update(TEntity entityToUpdate, params Expression<Func<TEntity, object>>[] includes);
    }
}