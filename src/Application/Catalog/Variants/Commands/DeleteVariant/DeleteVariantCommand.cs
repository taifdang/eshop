using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Commands.DeleteVariant;

public record DeleteVariantCommand : IRequest<Unit>
{
    public int Id { get; init; }// variant id
    public int ProductId { get; init; }
}

public class DeleteVariantCommandHandler : IRequestHandler<DeleteVariantCommand, Unit>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    public DeleteVariantCommandHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }

    public async Task<Unit> Handle(DeleteVariantCommand request, CancellationToken cancellationToken)
    {
        var variant = await _productVariantRepository.FirstOrDefaultAsync(new VariantFilterSpec(request.ProductId, request.Id))
            ?? throw new EntityNotFoundException(nameof(ProductVariant), request.Id);

        await _productVariantRepository.DeleteAsync(variant, cancellationToken);

        return Unit.Value;
    }
}
