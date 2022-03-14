using JMT.UOWDataLayerPattern.Core.Access.Patterns.Uow;

namespace JMT.UOWDataLayerPattern.Core.Access.Patterns.Factory
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUow(bool trackChanges = true, bool enableLogging = false);
    }
}