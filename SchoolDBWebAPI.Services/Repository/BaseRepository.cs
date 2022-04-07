using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolDBWebAPI.Services.DBModels;
using SchoolDBWebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolDBWebAPI.Services.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbSet<TEntity> dbSet;
        internal SchoolDBContext context;

        public BaseRepository(SchoolDBContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
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
            return context.Entry(entity);
        }

        public virtual async Task DeleteByIdAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
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
            IQueryable<TEntity> query = dbSet;

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
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual bool IsExists(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

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
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public virtual async Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AnyAsync();
        }

        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties ??= string.Empty;
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties ??= string.Empty;
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            IQueryable<TEntity> query = dbSet;
            includeProperties ??= string.Empty;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (typeof(TEntity).GetProperty(includeProperty) == null)
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
    }
}