using System;
using System.Threading.Tasks;
using JMT.UOWDataLayerPattern.Core.Access.Common;
using JMT.UOWDataLayerPattern.Core.Access.Patterns.Repository;

namespace JMT.UOWDataLayerPattern.Core.Access.Patterns.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetEntityRepository<TEntity>()
            where TEntity : class, IEntity,
            new();

        Task Commit();
    }
}