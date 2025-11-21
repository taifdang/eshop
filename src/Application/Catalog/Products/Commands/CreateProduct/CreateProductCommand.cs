using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateProduct;

public record CreateProductCommand(Guid CategoryId, string Title, string Description) : IRequest<Guid>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IRepository<Product> _productRepository;

    public CreateProductHandler(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //var product = _mapper.Map<Domain.Entities.Product>(request);
        var product = new Product
        {
            CategoryId = request.CategoryId,
            Title = request.Title,
            Description = request.Description,
            Status = Domain.Enums.ProductStatus.Draft

        };
        await _productRepository.AddAsync(product, cancellationToken);
        return product.Id;
    }
}