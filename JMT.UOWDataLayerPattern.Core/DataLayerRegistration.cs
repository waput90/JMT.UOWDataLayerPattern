using JMT.UOWDataLayerPattern.Core.Access.Patterns.Factory;
using JMT.UOWDataLayerPattern.Core.Access.Patterns.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JMT.UOWDataLayerPattern.Core
{
    public static class DataLayerRegistration
    {
        public static IServiceCollection RegisterDataAccess<TDbContext>(this IServiceCollection serviceCollection)
            where TDbContext : BaseDbContext<TDbContext>
        {
            RegisterDataAccessLayerServices<TDbContext>(serviceCollection);
            return serviceCollection;
        }

        private static void RegisterDataAccessLayerServices<TDbContext>(IServiceCollection serviceCollection)
            where TDbContext : BaseDbContext<TDbContext>
        {
            serviceCollection.TryAddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            serviceCollection.TryAddTransient<IDbContext, TDbContext>();
            serviceCollection.TryAddTransient(typeof(IRepository<>), typeof(EntityRepository<>));
        }
    }
}