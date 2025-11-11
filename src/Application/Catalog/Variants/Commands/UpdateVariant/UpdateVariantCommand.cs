using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Commands.UpdateVariant;

public record UpdateVariantCommand : IRequest<Unit>
{
    public int Id { get; init; } // variant id
    public decimal RegularPrice { get; init; }
    public int Quantity { get; init; }
    public decimal? Percent { get; init; }
    public string? Sku { get; init; }
}

public class UpdateVariantCommandHandler : IRequestHandler<UpdateVariantCommand, Unit>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;

    public UpdateVariantCommandHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }

    public async Task<Unit> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var specification = new VariantFilterSpec(null, request.Id);
        var variant = await _productVariantRepository.FirstOrDefaultAsync(specification)
            ?? throw new EntityNotFoundException(nameof(ProductVariant), request.Id);

        variant.Sku = variant.Sku;
        variant.RegularPrice = request.RegularPrice;
        variant.Quantity = request.Quantity;
        //no update variant option value, percent

        await _productVariantRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}