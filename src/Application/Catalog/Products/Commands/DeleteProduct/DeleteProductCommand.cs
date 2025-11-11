using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using MediatR;

namespace Application.Catalog.Products.Commands.DeleteProduct;

public record DeleteProductCommand : IRequest<Unit>
{
    public int Id { get; init; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IRepository<Domain.Entities.Product> _productRepository;

    public DeleteProductCommandHandler(IRepository<Domain.Entities.Product> productRepository)
    {
        _productRepository = productRepository;
    }


    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FirstOrDefaultAsync(new ProductFilterSpec(request.Id))
            ?? throw new EntityNotFoundException(nameof(Products), request.Id);

        await _productRepository.DeleteAsync(product, cancellationToken);

        return Unit.Value;
    }
}