using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Commands.DeleteVariant;

public record DeleteVariantCommand(Guid Id, Guid ProductId) : IRequest<Unit>;

public class DeleteVariantCommandHandler : IRequestHandler<DeleteVariantCommand, Unit>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    public DeleteVariantCommandHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }

    public async Task<Unit> Handle(DeleteVariantCommand request, CancellationToken cancellationToken)
    {
        var spec = new ProductVariantSpec()
            .ByVariantId(request.Id)
            .ByProductId(request.ProductId);

        var variant = await _productVariantRepository.FirstOrDefaultAsync(spec);
        Guard.Against.NotFound(request.Id, variant);

        await _productVariantRepository.DeleteAsync(variant, cancellationToken);

        return Unit.Value;
    }
}
