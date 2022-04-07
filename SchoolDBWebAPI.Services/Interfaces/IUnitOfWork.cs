using System;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void BeginTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}