using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JMT.UOWDataLayerPattern.Core.Access.Common;
using JMT.UOWDataLayerPattern.Core.Access.Patterns.Repository;

namespace JMT.UOWDataLayerPattern.Core.Access.Patterns.Uow
{
    public class UnitOfWork : BaseUnitOfWork<DbContext>, IUnitOfWork
    {
        protected bool _disposed = false;

        public UnitOfWork(
            DbContext dbContext,
            IServiceProvider serviceProvider)
            : base(dbContext, serviceProvider)
        {

        }

        public IRepository<TEntity> GetEntityRepository<TEntity>()
            where TEntity : class, IEntity,
            new()
        {
            var repository = new EntityRepository<TEntity>(_dbContext);
            if (repository == null)
                throw new ArgumentException("Repository not found.");

            return repository;
        }

        public async Task Commit()
        {
            if (CheckDisposed())
                throw new ObjectDisposedException(nameof(UnitOfWork));

            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Check if the UnitOfWork has been disposed
        /// </summary>
        /// <returns>True when <see cref="Dispose()" /> as been called</returns>
        protected virtual bool CheckDisposed() => _disposed;

        ~UnitOfWork()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}