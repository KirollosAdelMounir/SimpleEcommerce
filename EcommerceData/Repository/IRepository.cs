using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceData.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(object id, params Expression<Func<T, object>>[] includes);
        Task<T> GetByUniqueDetail(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>> predicates = null, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAllSynchronous(Expression<Func<T, bool>> predicates = null, params Expression<Func<T, object>>[] includes);
        Task<T> AddAsync(T entity);
        Task DeleteAsync(Expression<Func<T, bool>> filter);
        Task DeleteAsync(object id);
        Task UpdateAsync(Expression<Func<T, bool>> filter, Action<T> updateAction);
        Task UpdateAsync(object id, Action<T> updateAction, params Expression<Func<T, object>>[] includes);
        Task BulkUpdateAsync(Expression<Func<T, bool>> filter, Action<T> updateAction);
        void ChangeEntityState(T entity, EntityState state);
        Task Save();
    }
}
