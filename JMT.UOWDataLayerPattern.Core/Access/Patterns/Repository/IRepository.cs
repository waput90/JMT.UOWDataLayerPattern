using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JMT.UOWDataLayerPattern.Core.Access.Patterns.Repository
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> FindAsync(params object[] keyValues);
        TEntity FindBy(Expression<Func<TEntity, bool>> predicate = null);
        bool ContainBy(Expression<Func<TEntity, bool>> predicate = null);
        TEntity Find(params object[] keyValues);
        Task<IEnumerable<TEntity>> ItemsWithAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> ItemsAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> GetTotal(Expression<Func<TEntity, bool>> predicate = null);
        IQueryable<TEntity> ExecuteQueryable(string query);
        void Insert(TEntity newItem);
        Task InsertAsync(TEntity newItem);
        void InsertBulk(IEnumerable<TEntity> newItems);
        void Update(TEntity item, params Expression<Func<TEntity, object>>[] excludeProperties);
        void Delete(TEntity item);
        void Delete(object itemId);
        void DeleteBy(Expression<Func<TEntity, bool>> predicate = null);
        void DeleteBulk(IEnumerable<TEntity> itemsToDelete);
        IEnumerable<TEntity> ExecuteQuery(string query);
        IEnumerable<TEntity> ExecuteQuery(FormattableString query);
        bool ExecuteNonQuery(string query);
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> ItemsWithAsyncQueryable(
            Expression<Func<TEntity, bool>> predicate = null, 
            params Expression<Func<TEntity, object>>[] includeProperties);
    }
}