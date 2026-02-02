using Domain.Repositories;
using Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Services;

public class CrudService<T> : ICrudService<T> where T : Entity<Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    protected readonly IRepository<T, Guid> _repository;
    public CrudService(IRepository<T, Guid> repository)
    {
        _unitOfWork = repository.UnitOfWork;
        _repository = repository;
    }
    public Task<List<T>> GetAsync(CancellationToken cancellationToken = default)
    {
        return _repository.ToListAsync(_repository.GetQueryableSet());
    }

    public Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if(id == Guid.Empty)
        {
            throw new ValidationException("Invalid Id");
        }

        return _repository.FirstOrDefaultAsync( _repository.GetQueryableSet().Where(x => x.Id == id));
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        if(entity.Id.Equals(default))
        {
            return AddAsync(entity, cancellationToken);
        }
        else
        {
            return UpdateAsync(entity, cancellationToken);
        }
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _repository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
