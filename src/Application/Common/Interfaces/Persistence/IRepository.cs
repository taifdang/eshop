using Ardalis.Specification;

namespace Application.Common.Interfaces.Persistence;

public interface IRepository<T> : IRepositoryBase<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }
}
