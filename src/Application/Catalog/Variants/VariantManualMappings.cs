using Application.Catalog.Variants.Queries.GetVariantById;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Catalog.Variants;

public static class VariantManualMappings
{
    public static VariantDto ToVariantDto(this ProductVariant variant)
    {
        return new VariantDto
        {
            Id = variant.Id,
            ProductId = variant.ProductId,
            ProductName = variant.Product.Title,
            Title = variant.Title,
            Price = variant.Price,
            Percent = variant.Percent,
            Quantity = variant.Quantity,
            Sku = variant.Sku,
            Image = variant.GetVariantImage(),
            Options = GetVariantOptionValue(variant)
        };
    }

    public static ImageLookupDto? GetVariantImage(this ProductVariant variant)
    {
        var image = 
            variant.VariantOptionValues
                .SelectMany(vov => vov.OptionValue.ProductImages!
                    .Where(pi => pi.OptionValueId == vov.OptionValueId))
                .OrderBy(x => x.Id)
                .FirstOrDefault();

        return image is null
            ? null
            : new ImageLookupDto { Id = image.Id, Url = image.ImageUrl};
    }

    public static ImageLookupDto? GetMainImage(this ProductVariant variant)
    {
        var image = variant.Product.ProductImages
            .FirstOrDefault();

        return image is null
            ? null
            : new ImageLookupDto { Id = image.Id, Url = image.ImageUrl };
    }

    private static List<VariantOptionValueDto> GetVariantOptionValue(ProductVariant variant)
    {
        return variant.VariantOptionValues
            .Select(vov => new VariantOptionValueDto
            {
                Title = vov.OptionValue.ProductOption.OptionName,
                Value = vov.OptionValue.Value
            })
            .ToList();
    }
}
