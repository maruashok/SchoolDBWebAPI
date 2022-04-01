using System;

namespace SchoolDBWebAPI.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();

        int SaveChanges();

        void BeginTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}