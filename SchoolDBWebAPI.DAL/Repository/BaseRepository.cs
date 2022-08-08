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
    public abstract class BaseRepository<TEntity> : ProcedureManager, IBaseRepository<TEntity> where TEntity : class
    {
        private readonly SchoolDBContext dBContext;
        private readonly DbSet<TEntity> dbSet;
        private IDbContextTransaction transaction;

        public BaseRepository(SchoolDBContext context) : base(context)
        {
            this.dBContext = context;
            this.dbSet = context.Set<TEntity>();
        }

        public void BeginTransaction()
        {
            transaction = dBContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (dBContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void DeleteById(object id)
        {
            Delete(dbSet.Find(id));
        }

        public virtual async Task DeleteByIdAsync(object id)
        {
            Delete(await dbSet.FindAsync(id));
        }

        public virtual void DeleteRange(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = GetQueryable(filter);

            if (query.Any())
            {
                query.ToList().ForEach(item => Delete(item));
            }
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, bool>>[] includes)
        {
            IQueryable<TEntity> query = GetQueryable(filter);
            query = includes.Aggregate(query, (current, include) => current.Include(include));

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

        public virtual IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = GetQueryable();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIDAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable(filter);
            return query.Count();
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable(filter);
            return await query.CountAsync();
        }

        public virtual EntityEntry<TEntity> GetEntityEntry(TEntity entity)
        {
            return dBContext.Entry(entity);
        }

        public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = GetQueryable(filter);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return query.FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = GetQueryable(filter);
            query = includes.Aggregate(query, (current, include) => current.Include(include));
            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public virtual void InsertRange(List<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual async Task InsertRangeAsync(List<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public virtual bool IsExists(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable(filter);
            return query.Any();
        }

        public virtual async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = GetQueryable(filter);
            return await query.AnyAsync();
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

        public virtual void SetEntityValues(TEntity entity, object Values)
        {
            GetEntityEntry(entity).CurrentValues.SetValues(Values);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            dBContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entityToUpdate, params Expression<Func<TEntity, object>>[] properties)
        {
            if (properties != null)
            {
                foreach (var currentProperty in properties)
                {
                    string propertyName = currentProperty.GetPropertyName();

                    if (!string.IsNullOrEmpty(propertyName) && entityToUpdate.GetPropertyValue(propertyName, out object entityValue))
                    {
                        if (entityValue != null)
                        {
                            if (entityValue is IEnumerable enumerable)
                            {
                                foreach (var item in enumerable)
                                {
                                    if (item.GetType().IsClass)
                                    {
                                        dBContext.Attach(item);
                                        dBContext.Entry(item).State = EntityState.Modified;
                                    }
                                }
                            }
                            else
                            {
                                if (entityValue.GetType().IsClass)
                                {
                                    dBContext.Attach(entityValue);
                                    dBContext.Entry(entityValue).State = EntityState.Modified;
                                }
                            }
                        }
                    }
                }
            }

            dbSet.Attach(entityToUpdate);
            dBContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}