using System;
using Microsoft.EntityFrameworkCore;
using JMT.UOWDataLayerPattern.Core.Access.Patterns.Uow;

namespace JMT.UOWDataLayerPattern.Core.Access.Patterns.Factory
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork CreateUow(bool trackChanges = true, bool enableLogging = false)
        {
            DbContext dbContext = (DbContext)_serviceProvider.GetService(typeof(IDbContext));

            if (!trackChanges)
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return new UnitOfWork(dbContext, _serviceProvider) as IUnitOfWork;
        }
    }
}