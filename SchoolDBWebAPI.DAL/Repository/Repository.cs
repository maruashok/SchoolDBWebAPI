using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SchoolDBWebAPI.DAL.DBModels;
using SchoolDBWebAPI.DAL.Helpers;
using SchoolDBWebAPI.DAL.Interfaces;
using SchoolDBWebAPI.DAL.SPHelper;
using System.Collections;
using System.Linq.Expressions;

namespace SchoolDBWebAPI.DAL.Repository
{
    public class Repository<TEntity> : ProcedureManager, IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet;
        private readonly DbContext dBContext;
        private IDbContextTransaction? transaction;

        public Repository(DbContext context) : base(context)
        {
            this.dBContext = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete != null)
            {
                if (dBContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }

                dbSet.Remove(entityToDelete);
            }
        }

        public virtual void DeleteById(object id)
        {
            TEntity? entity = dbSet.Find(id);

            if (entity != null)
            {
                Delete(entity);
            }
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter, bool AsNoTracking = false)
        {
            IQueryable<TEntity> query = AsNoTracking ? dbSet.AsNoTracking() : dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public virtual async void DeleteByIdAsync(object id)
        {
            TEntity? entity = await dbSet.FindAsync(id);

            if (entity != null)
            {
                Delete(entity);
            }
        }

        public IQueryable<TEntity> GetPagedResponseAsync(int pageNumber, int pageSize)
        {
            return dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter, bool AsNoTracking = false)
        {
            return GetQueryable(filter, AsNoTracking);
        }

        public virtual void Insert(TEntity entity)
        {
            if (entity != null)
            {
                dbSet.Add(entity);
            }
        }

        public virtual void InsertAsync(TEntity entity)
        {
            if (entity != null)
            {
                dbSet.AddAsync(entity);
            }
        }

        public virtual void InsertRange(List<TEntity> entities)
        {
            if (entities != null)
            {
                dbSet.AddRange(entities);
            }
        }

        public virtual async Task InsertRangeAsync(List<TEntity> entities)
        {
            if (entities != null)
            {
                await dbSet.AddRangeAsync(entities);
            }
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate != null)
            {
                dbSet.Attach(entityToUpdate);
                dBContext.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }

        public void BeginTransaction()
        {
            transaction = dBContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            transaction?.Commit();
        }

        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }
        }

        public int SaveChanges()
        {
            return dBContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return dBContext.SaveChangesAsync();
        }
    }
}