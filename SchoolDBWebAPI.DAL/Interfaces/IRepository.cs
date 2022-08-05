using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolDBWebAPI.DAL.SPHelper;
using System.Linq.Expressions;

namespace SchoolDBWebAPI.DAL.Interfaces
{
    public interface IRepository<TEntity> : IProcedureManager where TEntity : class
    {
        void BeginTransaction();

        void Commit();

        void Delete(TEntity entityToDelete);

        void DeleteById(object id);

        Task DeleteByIdAsync(object id);

        void DeleteRange(Expression<Func<TEntity, bool>> filter);

        void Update(TEntity entityToUpdate, string ChildEntities);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null);

        TEntity GetByID(object id);

        Task<TEntity> GetByIDAsync(object id);

        int GetCount(Expression<Func<TEntity, bool>> filter = null);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);

        EntityEntry<TEntity> GetEntityEntry(TEntity entity);

        Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null);

        TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null);

        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null);

        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);

        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        IQueryable<TEntity> GetQueryable();

        void InsertRange(List<TEntity> entities);

        Task InsertRangeAsync(List<TEntity> entities);

        bool IsExists(Expression<Func<TEntity, bool>> filter = null);

        void Rollback();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void SetEntityValues(TEntity entity, object Values);

        void Update(TEntity entityToUpdate);
    }
}