using SchoolDBWebAPI.DBModels;
using System;

namespace SchoolDBWebAPI.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        IRepository<QuizDetail> QuizDetailRepository { get; }
    }
}