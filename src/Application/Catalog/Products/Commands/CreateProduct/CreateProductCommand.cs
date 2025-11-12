using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<int>
{
    public int CategoryId { get; init; }
    public string Title { get; init; }
    //public decimal MinPrice { get; init; }
    //public decimal MaxPrice { get; init; }
    //public int Quantity { get; init; }
    public string Description { get; init; }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IMapper _mapper;

    public CreateProductHandler(IRepository<Product> productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //var product = _mapper.Map<Domain.Entities.Product>(request);
        var product = new Product
        {
            CategoryId = request.CategoryId,
            Title = request.Title,
            Description = request.Description,
            IsPublished = false

        };
        await _productRepository.AddAsync(product, cancellationToken);
        return product.Id;
    }
}