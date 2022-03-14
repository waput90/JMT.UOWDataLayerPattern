using Microsoft.EntityFrameworkCore;

namespace JMT.UOWDataLayerPattern.Core
{
    public abstract class BaseDbContext<T> : DbContext, IDbContext where T : DbContext
    {
        protected BaseDbContext(DbContextOptions<T> contextOptions) : base(contextOptions)
        {

        }
    }
}
