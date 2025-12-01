using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Title, string? Label) : IRequest<Guid>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IRepository<Category> _categoryRepo;
    public CreateCategoryCommandHandler(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }
    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Title = request.Title,
            Label = request.Label,
        };

        await _categoryRepo.AddAsync(category);

        return category.Id;
    }
}
