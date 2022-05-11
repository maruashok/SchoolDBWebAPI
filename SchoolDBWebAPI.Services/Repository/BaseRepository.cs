using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.SPHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SchoolDBWebAPI.Services.Extensions;

namespace SchoolDBWebAPI.Services.Repository
{
    public abstract class BaseRepository<TEntity> : ProcedureManager, IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> dbSet;
        private IDbContextTransaction transaction;
        private readonly SchoolDBContext dBContext;

        public BaseRepository(SchoolDBContext context) : base(context)
        {
            this.dBContext = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return dbSet.AsNoTracking();
        }

        public virtual void DeleteById(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void SetEntityValues(TEntity entity, object Values)
        {
            GetEntityEntry(entity).CurrentValues.SetValues(Values);
        }

        public virtual EntityEntry<TEntity> GetEntityEntry(TEntity entity)
        {
            return dBContext.Entry(entity);
        }

        public virtual async Task DeleteByIdAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (dBContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            dBContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entityToUpdate, string ChildEntities)
        {
            if (!string.IsNullOrEmpty(ChildEntities))
            {
                foreach (string updateProperty in ChildEntities.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    PropertyInfo property = typeof(TEntity).GetProperty(updateProperty);

                    if (property != null && property.GetValue(entityToUpdate) != null)
                    {
                        object entityValue = property.GetValue(entityToUpdate);

                        if (entityValue is IEnumerable)
                        {
                            foreach (var item in entityValue as IEnumerable)
                            {
                                dBContext.Attach(item);
                                dBContext.Entry(item).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            dBContext.Attach(entityValue);
                            dBContext.Entry(entityValue).State = EntityState.Modified;
                        }
                    }
                }
            }

            dbSet.Attach(entityToUpdate);
            dBContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual void InsertRange(List<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task InsertRangeAsync(List<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public virtual void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
                if (query.Any())
                {
                    query.ToList().ForEach(item => Delete(item));
                }
            }
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual bool IsExists(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Any();
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public virtual async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AnyAsync();
        }

        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties ??= string.Empty;
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (typeof(TEntity).GetProperty(includeProperty) != null)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties ??= string.Empty;
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (typeof(TEntity).GetProperty(includeProperty) != null)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            IQueryable<TEntity> query = GetQueryable();
            includeProperties ??= string.Empty;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (typeof(TEntity).GetProperty(includeProperty) != null)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public void BeginTransaction()
        {
            transaction = dBContext.Database.BeginTransaction();
        }

        public int SaveChanges()
        {
            return dBContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return dBContext.SaveChangesAsync();
        }

        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }
        }
    }
}