using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Catalog.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IRepository<Category, Guid> _repository;

    public CreateCategoryCommandHandler(IRepository<Category, Guid> repository)
    {
        _repository = repository;
    }
    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Title,
            UrlSlug = request.UrlSlug
        };

        await _repository.AddAsync(category);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}