using SchoolDBWebAPI.DBModels;
using System;

namespace SchoolDBWebAPI.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();

        int SaveChanges();

        void BeginTransaction();

        IRepository<QuizDetail> QuizDetailRepository { get; }
    }
}