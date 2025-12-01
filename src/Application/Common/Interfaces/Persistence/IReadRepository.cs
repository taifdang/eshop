using Ardalis.Specification;

namespace Application.Common.Interfaces.Persistence;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
{
}
