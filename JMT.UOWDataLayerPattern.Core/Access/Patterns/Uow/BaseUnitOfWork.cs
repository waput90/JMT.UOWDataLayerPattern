using Microsoft.EntityFrameworkCore;
using System;

namespace JMT.UOWDataLayerPattern.Core.Access.Patterns.Uow
{
    public class BaseUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        protected TDbContext _dbContext;
        protected readonly IServiceProvider _serviceProvider;

        protected BaseUnitOfWork(
            TDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }
    }
}