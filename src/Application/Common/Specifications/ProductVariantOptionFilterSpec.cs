using Application.Catalog.Variants.Queries.GetVariantById;
using Application.Catalog.Variants.Queries.GetVariantByOption;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class ProductVariantOptionFilterSpec : Specification<ProductVariant, VariantItemDto>
{
    public ProductVariantOptionFilterSpec(
        Guid productId, 
        List<Guid> optionValues, 
        bool exactMatch)
    {
        Query.Where(c => c.ProductId == productId);   

        if (optionValues.Any())
        {
            // Not enough to option values, need to match all provided options
            Query.Where(x => optionValues.All(opt => x.VariantOptionValues.Any(v => v.OptionValue.Id == opt)));
            // Corrected to ensure exact match of option values
            // Ex: Color : Red, Size: M  should not match Color: Red, Size: M, Material: Cotton
            if (exactMatch)
                Query.Where(x => x.VariantOptionValues.Count == optionValues.Count);
        }

        Query.Select(x => new VariantItemDto
             {
                Id = x.Id,
                Price = x.Price,
                Percent = x.Percent,
                Quantity = x.Quantity,
                Sku = x.Sku ?? string.Empty,
                Options = x.VariantOptionValues.Select(y => new VariantOptionValueDto
                {
                    Title = y.OptionValue.ProductOption.OptionName,
                    Value = y.OptionValue.Value
                })
                .OrderBy(o => o.Title)
                .ToList()
             });
    }

}