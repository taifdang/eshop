using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using MediatR;

namespace Application.Catalog.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public int CategoryId { get; init; }
    public string Title { get; init; }
    //public decimal MinPrice { get; init; }
    //public decimal MaxPrice { get; init; }
    //public int Quantity { get; init; }
    public string Description { get; init; }
}   

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IRepository<Domain.Entities.Product> _productRepository;

    public UpdateProductCommandHandler(IRepository<Domain.Entities.Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FirstOrDefaultAsync(new ProductFilterSpec(request.Id))
            ?? throw new EntityNotFoundException(nameof(Products), request.Id);

        product.CategoryId = request.CategoryId;
        product.Title = request.Title;
        product.Description = request.Description;

        await _productRepository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

