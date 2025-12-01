using Application.Catalog.Products.Specifications;
using Application.Common.Interfaces.Persistence;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Products.Queries.GetVariantByOption;

public record GetVariantByOptionQuery(Guid ProductId, List<Guid> OptionValueMap) : IRequest<AvailableVariant>;

public class GetVariantByOptionQueryHandler : IRequestHandler<GetVariantByOptionQuery, AvailableVariant>
{
    private readonly IRepository<ProductVariant> _productVariantRepository;
    private readonly IRepository<ProductOption> _productOptionRepository;

    public GetVariantByOptionQueryHandler(
        IRepository<ProductVariant> productVariantRepository,
        IRepository<ProductOption> productOptionRepository)
    {
        _productVariantRepository = productVariantRepository;
        _productOptionRepository = productOptionRepository;
    }
    public async Task<AvailableVariant> Handle(GetVariantByOptionQuery request, CancellationToken cancellationToken)
    {
        var optionSpec = new ProductOptionSpec().ByProductId(request.ProductId);

        var optionCount = await _productOptionRepository.CountAsync(
            optionSpec, 
            cancellationToken);

        // boolean
        var exact = request.OptionValueMap.Count == optionCount;

        var variantSpec = new ProductVariantSpec()
            .ByProductId(request.ProductId)
            .FilterByOptions(request.OptionValueMap, exact)
            .WithProjectionOf(new ProductVariantItemProjectionSpec());

        var variants = await _productVariantRepository.ListAsync(variantSpec, cancellationToken);

        if (!variants.Any())
            Guard.Against.NotFound(nameof(variants),variants);

        return new AvailableVariant
        {
            Variants = variants,
            MinPrice = variants.Min(v => v.Price),
            MaxPrice = variants.Max(v => v.Price),
            TotalStock = variants.Sum(v => v.Quantity)
        };
    }
}
