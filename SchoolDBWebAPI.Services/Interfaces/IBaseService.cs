using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Services
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        int Delete(TEntity entityToDelete);
        int DeleteById(object id);
        Task<int> DeleteByIdAsync(object id);
        int DeleteRange(Expression<Func<TEntity, bool>> filter);
        Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> filter);
        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);
        int InsertRange(List<TEntity> entities);
        Task<int> InsertRangeAsync(List<TEntity> entities);
        bool Update(TEntity entityToUpdate);
        Task<bool> UpdateAsync(TEntity entityToUpdate);
    }
}