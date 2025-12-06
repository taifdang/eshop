using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            UrlSlug = request.UrlSlug,
            Description = request.Description,
            IsActive = false,
            IsDeleted = false
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}