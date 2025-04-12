using EcommerceData.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceData.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> _entity;
        private EcommerceContext _dbContext;
        public Repository(EcommerceContext context)
        {
            _dbContext = context;
            _entity = _dbContext.Set<T>();
        }
        public virtual async Task<T> AddAsync(T entity)
        {
            var addedEntityTrack = await _dbContext.AddAsync(entity);
            await Save();
            return addedEntityTrack.Entity;
        }

        public virtual async Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            _dbContext.Remove(filter);
            await Save();
        }
        public virtual async Task DeleteAsync(object id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                _dbContext.Remove(entity);
                await Save();
            }
        }
        public virtual async Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicates = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entity;

            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            // Apply each predicate to the query
            if (predicates != null)
            {
                query = query.Where(predicates);
            }

            return query;
        }
        public virtual IQueryable<T> GetAllSynchronous(Expression<Func<T, bool>> predicates = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entity;

            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicates != null)
            {
                query = query.Where(predicates);
            }

            return query; // No async needed, just return IQueryable<T>
        }

        public virtual async Task<T> GetByUniqueDetail(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entity;

            // Apply includes
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            var record = await query.FirstOrDefaultAsync(predicate);
            if (record != null)
            {
                return record;
            }
            return null;
        }
        public virtual async Task<T> GetById(object id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _entity;

            // Apply includes
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var entity = await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
            if (entity != null)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
                return entity;
            }

            throw new Exception("Missing Entity");
        }
        public virtual async Task UpdateAsync(Expression<Func<T, bool>> filter, Action<T> updateAction)
        {
            var entity = await GetByUniqueDetail(filter);
            if (entity != null)
            {
                updateAction(entity);
                await Save();
                _dbContext.Entry(entity).State = EntityState.Detached;
            }
        }
        public virtual async Task UpdateAsync(object id, Action<T> updateAction, params Expression<Func<T, object>>[] includes)
        {
            var entity = await GetById(id, includes);
            updateAction(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Save();
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
        public virtual async Task BulkUpdateAsync(Expression<Func<T, bool>> filter, Action<T> updateAction)
        {
            var entities = await GetAll(filter); // Fetch entities based on filter

            foreach (var entity in entities)
            {
                updateAction(entity); // Apply the update action
            }

            _dbContext.UpdateRange(entities); // Mark all entities as updated
            await Save();// Persist changes to the database
        }
        
        public async Task Save()
        {
            var x = await _dbContext.SaveChangesAsync();
        }

        public void ChangeEntityState(T entity, EntityState state)
        {
            _dbContext.Entry(entity).State = state;
        }
    }
}
