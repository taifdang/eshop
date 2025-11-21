using Application.Catalog.Variants.Queries.GetVariantById;
using Application.Common.Models;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class ProductVariantByIdSpec : Specification<ProductVariant, VariantVm>
{
    public ProductVariantByIdSpec(Guid productVariantId)
    {
        Query
            .Where(x => x.Id == productVariantId)
            .AsNoTracking();

        Query.Select(x => new VariantVm
        {
            Id = x.Id,
            Title = x.Title ?? "",
            ProductId = x.ProductId,
            ProductName = x.Product.Title,
            Price = x.Price,
            Percent = x.Percent,
            Quantity = x.Quantity,
            Sku = x.Sku ?? "",
            Image = null,
            Options = x.VariantOptionValues.Select(y => new OptionLookupDto
            {
                Title = y.OptionValue.ProductOption.OptionName,
                OptionValueId = y.OptionValueId,
                Value = y.OptionValue.Value,
                IsImage = y.OptionValue.ProductOption.AllowImage
            }).ToList()
        });
    }
}
