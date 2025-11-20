using Application.Catalog.Variants.Dtos;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class ProductVariantOptionFilterSpec : Specification<ProductVariant>
{
    public ProductVariantOptionFilterSpec(
        Guid productId, 
        List<Guid> optionValues, 
        bool exact)
    {
        Query
            .Where(c => c.ProductId == productId)
            .Include(x => x.Product)
            .Include(x => x.VariantOptionValues)
                .ThenInclude(v => v.OptionValue)
                    .ThenInclude(ov => ov.ProductImages)
            .Include(x => x.VariantOptionValues)
                .ThenInclude(v => v.OptionValue)
                    .ThenInclude(ov => ov.ProductOption);

        if (optionValues != null && optionValues.Count > 0)
        {
            // Not enough to option values, need to match all provided options
                Query
                    .Where(x =>
                        optionValues.All(opt =>
                            x.VariantOptionValues.Any(v =>
                                v.OptionValue.Id == opt))
                        );

            if (exact)
            {
                // Corrected to ensure exact match of option values
                // Ex: Color : Red, Size: M  should not match Color: Red, Size: M, Material: Cotton
                Query
                  .Where(x => x.VariantOptionValues.Count == optionValues.Count);

            }
        }

        //Query
        //    .Select(x => new VariantDto(
        //        x.Id,
        //        x.ProductId,
        //        x.Product.Title,
        //        x.Title ?? "",
        //        x.RegularPrice,
        //        x.Percent,
        //        x.Quantity,
        //        x.Sku ?? "",
        //        x.VariantOptionValues
        //            .SelectMany(vov => vov.OptionValue.ProductImages!
        //                .Where(img => img.OptionValueId == vov.OptionValueId))
        //            .OrderBy(img => img.Id)
        //            .Select(img => new VariantImageDto(img.Id, img.ImageUrl))
        //            .FirstOrDefault(),
        //        x.VariantOptionValues
        //            .Select(y => new VariantOptionValueDto(y.OptionValue.ProductOption.OptionName, y.OptionValue.Value))
        //            .ToList()));


        //.Select(x => new VariantDto
        // {
        //     Id = x.Id,
        //     ProductId = productId,
        //     ProductName = x.Product.Title,
        //     Title = x.Title ?? string.Empty,
        //     RegularPrice = x.RegularPrice,
        //     //MaxPrice = x.MaxPrice,
        //     Percent = x.Percent,
        //     Quantity = x.Quantity,
        //     Sku = x.Sku ?? string.Empty,
        //     Image = x.VariantOptionValues
        //                .SelectMany(vov => vov.OptionValue.ProductImages!
        //                .Where(img => img.OptionValueId == vov.OptionValueId))
        //                .OrderBy(img => img.Id)
        //                .Select(img => new VariantImageDto
        //                {
        //                    Id = img.Id,
        //                    Url = img.ImageUrl
        //                })
        //                .FirstOrDefault(),
        //     Options = x.VariantOptionValues.Select(y => new VariantOptionValueDto
        //     {
        //         Title = y.OptionValue.ProductOption.OptionName,
        //         Value = y.OptionValue.Value
        //     }).ToList()
        // });
    }
}
