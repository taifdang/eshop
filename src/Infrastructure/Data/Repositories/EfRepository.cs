using Application.Common.Interfaces.Persistence;
using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
{
    public IUnitOfWork UnitOfWork { get; }
    public EfRepository(ApplicationDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
    {
        UnitOfWork = unitOfWork;
    }
}
