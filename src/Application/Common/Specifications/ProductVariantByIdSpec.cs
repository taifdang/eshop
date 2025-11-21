using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class ProductVariantByIdSpec : Specification<ProductVariant>
{
    public ProductVariantByIdSpec(Guid productVariantId)
    {
        Query       
            .Where(x => x.Id == productVariantId)
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Product)
            .Include(x => x.VariantOptionValues)
                .ThenInclude(vov => vov.OptionValue)
                    .ThenInclude(ov => ov.ProductImages)
            .Include(x => x.VariantOptionValues)
                .ThenInclude(vov => vov.OptionValue)
                    .ThenInclude(po => po.ProductOption);

        //Query
        //    .Select(x => new VariantDto(
        //        x.Id,
        //        x.ProductId,
        //        x.Product.Title,
        //        x.Title ?? "",
        //        x.Price,
        //        x.Percent,
        //        x.Quantity,
        //        x.Sku ?? "",
        //        x.VariantOptionValues
        //            .SelectMany(vov => vov.OptionValue.ProductImages!
        //                .Where(img => img.OptionValueId == vov.OptionValueId))
        //            .Select(img => new VariantImageDto(img.Id, img.ImageUrl))
        //            .OrderBy(img => img.Id)
        //            .FirstOrDefault(),
        //        x.VariantOptionValues
        //            .Select(y => new VariantOptionValueDto(y.OptionValue.ProductOption.OptionName, y.OptionValue.Value))
        //            .ToList()));

        /// All in
        //Image = (
        //    from img in x.Product.ProductImages
        //    where img.ProductOptionId != null &&
        //          x.VariantOptionValues
        //           .Select(vov => vov.ProductOptionId)
        //           .Contains(img.ProductOptionId.Value)
        //    orderby img.ProductOptionId
        //    select new VariantImageDto
        //    {
        //        ProductOptionId = img.ProductOptionId,
        //        Url = img.ImageUrl!
        //    }
        //).FirstOrDefault() ??
        //(
        //    from img in x.Product.ProductImages
        //    where img.IsMain && img.ProductOptionId == null
        //    select new VariantImageDto
        //    {
        //        ProductOptionId = img.ProductOptionId,
        //        Url = img.ImageUrl!
        //    }
        //).FirstOrDefault() ?? new VariantImageDto(),


        //Query.Select(x => new VariantDto
        // {
        //     Id = x.Id,
        //     Title = x.Title ?? string.Empty,
        //     ProductId = x.ProductId,
        //     ProductName = x.Product.Title,
        //     Price = x.Price,
        //     Percent = x.Percent,
        //     Quantity = x.Quantity,
        //     Sku = x.Sku ?? string.Empty,
        //     Image = x.VariantOptionValues
        //                .SelectMany(vov => vov.OptionValue.ProductImages!
        //                    .Where(img => img.OptionValueId == vov.OptionValueId))
        //                .Select(img => new VariantImageDto
        //                {
        //                    Id = img.Id,
        //                    Url = img.ImageUrl
        //                })
        //                .OrderBy(img => img.Id)
        //                .FirstOrDefault() ?? new(),
        //     Options = x.VariantOptionValues.Select(y => new VariantOptionValueDto
        //     {
        //         Title = y.OptionValue.ProductOption.OptionName,
        //         Value = y.OptionValue.Value
        //     }).ToList()
        // });
    }
}
