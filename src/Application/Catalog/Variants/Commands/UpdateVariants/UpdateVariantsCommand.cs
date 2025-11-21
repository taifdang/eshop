using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Commands.UpdateVariants;

public record UpdateVariantsCommand(Guid ProductId, decimal? Price, int? Quantity, string? Sku) : IRequest<Guid>;

public class UpdateVariantsCommandHandler : IRequestHandler<UpdateVariantsCommand, Guid>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;

    public UpdateVariantsCommandHandler(IRepository<ProductVariant> productVariantRepository)
    {
        _productVariantRepository = productVariantRepository;
    }

    public async Task<Guid> Handle(UpdateVariantsCommand request, CancellationToken cancellationToken)
    {
        var variants = await _productVariantRepository.ListAsync(new VariantByProductIdSpec(request.ProductId))
            ?? throw new EntityNotFoundException(nameof(Product), "null");

        foreach (var v in variants)
        {
            if (request.Price.HasValue)
            {
                v.Price = request.Price.Value;
            }
            if (request.Quantity.HasValue)
            {
                v.Quantity = request.Quantity.Value;
            }
            if(!string.IsNullOrWhiteSpace(request.Sku)) 
            { 
                v.Sku = request.Sku;
            }       
        }

        await _productVariantRepository.UpdateRangeAsync(variants, cancellationToken);

        return request.ProductId;
    }
}


