using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace SchoolDBWebAPI.DAL.Interfaces
{
    public interface IRepository<TEntity> : IProcedureManager where TEntity : class
    {
        void Delete(TEntity entityToDelete);

        void DeleteById(object id);

        void DeleteByIdAsync(object id);

        IQueryable<TEntity> GetPagedResponseAsync(int pageNumber, int pageSize);

        IQueryable<TEntity> Query(bool AsNoTracking);

        void Insert(TEntity entity);

        void InsertAsync(TEntity entity);

        void InsertRange(List<TEntity> entities);

        Task InsertRangeAsync(List<TEntity> entities);

        void Update(TEntity entityToUpdate);
    }
}