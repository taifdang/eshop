using Application.Common.Interfaces.Persistence;
using Ardalis.GuardClauses;
using MediatR;

namespace Application.Catalog.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IRepository<Domain.Entities.Product> _productRepository;

    public DeleteProductCommandHandler(IRepository<Domain.Entities.Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        Guard.Against.NotFound(request.Id, product);
         
        await _productRepository.DeleteAsync(product, cancellationToken);

        return Unit.Value;
    }
}