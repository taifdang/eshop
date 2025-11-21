using Application.Common.Interfaces;
using Application.Common.Specifications;
using Ardalis.GuardClauses;
using Domain.Entities;
using MediatR;

namespace Application.Catalog.Variants.Queries.GetVariantByOption;

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
        var optionCount = await _productOptionRepository.CountAsync(
            new ProductOptionFilterSpec(request.ProductId, null), 
            cancellationToken);

        var exact = request.OptionValueMap.Count == optionCount;

        var spec = new ProductVariantOptionFilterSpec(request.ProductId, request.OptionValueMap, exact);
        var variants = await _productVariantRepository.ListAsync(spec, cancellationToken);

        if (!variants.Any())
            Guard.Against.NotFound(nameof(Variants),variants);

        return new AvailableVariant
        {
            Variants = variants,
            MinPrice = variants.Min(v => v.Price),
            MaxPrice = variants.Max(v => v.Price),
            TotalStock = variants.Sum(v => v.Quantity)
        };
    }
}
