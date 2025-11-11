using Application.Common.Interfaces;
using AutoMapper;
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
    private readonly IRepository<Domain.Entities.Product> _productRepository;
    private readonly IMapper _mapper;

    public CreateProductHandler(IRepository<Domain.Entities.Product> productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Domain.Entities.Product>(request);
        await _productRepository.AddAsync(product, cancellationToken);
        return product.Id;
    }
}