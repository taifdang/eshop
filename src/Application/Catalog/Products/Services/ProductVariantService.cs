using Application.Common.Dtos;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Services;

public class ProductVariantService : IProductVariantService
{
    private readonly IReadRepository<OptionValue, Guid> _readRepository;
    private readonly IVariantGenerator _variantGenerator;

    public ProductVariantService(
        IVariantGenerator variantGenerator,
        IReadRepository<OptionValue, Guid> readRepository)
    {
        _variantGenerator = variantGenerator;
        _readRepository = readRepository;
    }

    public async Task GenerateVariantsAsync(
        Product product,
        Dictionary<Guid, List<Guid>> optionValueMap,
        CancellationToken cancellationToken = default)
    {
        if (optionValueMap == null || optionValueMap.Count == 0)
            throw new ArgumentException("No option values provided.");

        // Get all option value IDs
        var optionValueIds = optionValueMap.SelectMany(x => x.Value).ToList();

        // Query option values
        var query = _readRepository
            .GetQueryableSet()
            .Where(m => optionValueIds.Contains(m.Id));

        var optionValue = await query
            .Select(m => new OptionValueDto(m.Id, m.Value))
            .ToListAsync(cancellationToken);

        // Create dictionary for lookup
        var optionValueDict = optionValue.ToDictionary(x => x.Id, x => x.Value);

        // Generate combinations
        var combinations = _variantGenerator.CartesianProduct(optionValueMap.Values).ToList();

        if (combinations.Count == 0)
            throw new InvalidOperationException("No combinations generated.");

        // Generate variants through domain method
        foreach (var item in combinations)
        {
            var nameList = item.ToList();
            var variantTitle = string.Join(" - ", nameList.Select(n => optionValueDict[n]));

            var variant = new Variant
            {
                ProductId = product.Id,
                Title = variantTitle,
                VariantOptions = nameList.Select(id => new VariantOption { OptionValueId = id }).ToList()
            };

            product.AddVariant(variant);
        }
    }
}
