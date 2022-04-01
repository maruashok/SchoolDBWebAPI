using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface ICommandService<TEntity> where TEntity : class
    {
        void Commit();

        void Rollback();

        int SaveChanges();

        void BeginTransaction();

        void Delete(TEntity entityToDelete);

        void DeleteById(object id);

        Task DeleteByIdAsync(object id);

        void DeleteRange(Expression<Func<TEntity, bool>> filter);

        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity);

        void InsertRange(List<TEntity> entities);

        Task InsertRangeAsync(List<TEntity> entities);

        void Update(TEntity entityToUpdate);
    }
}