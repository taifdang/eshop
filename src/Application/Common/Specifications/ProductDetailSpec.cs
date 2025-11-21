using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class ProductDetailSpec : Specification<Product>
{
    public ProductDetailSpec(Guid productId)
    {
        Query
            .Where(x => x.Id == productId)
            .Include(x => x.Category)
            .Include(x => x.ProductVariants)
            .Include(x => x.ProductImages)
            .Include(x => x.ProductOptions)
                .ThenInclude(x => x.OptionValues)
                    .ThenInclude(ov => ov.ProductImages)
            .AsSplitQuery()
            .AsNoTracking();

        //Query
        //    .Where(x => x.Id == productId)
        //    .Include(x => x.ProductOptions)
        //        .ThenInclude(x => x.OptionValues);

        //Query
        //    .Select(x => new ProductItemDto
        //    {
        //        Id = x.Id,
        //        Title = x.Title,
        //        MinPrice = x.ProductVariants.Min(x => x.Price),
        //        MaxPrice = x.ProductVariants.Max(x => x.Price),
        //        Description = x.Description ?? string.Empty,
        //        Category = x.Category.Title,
        //        Images = x.ProductImages.Select(img => new ImageLookupDto
        //        {
        //            Id = img.Id,
        //            Url = img.ImageUrl,
        //        }).ToList(),
        //        Options = x.ProductOptions.Select(po => new ProductOptionDto
        //        {
        //            Title = po.OptionName,
        //            Values = po.OptionValues.Select(v => v.Value).ToList()
        //        }).ToList(),
        //        OptionValues = x.ProductOptions.Select(po => new ProductOptionValueDto
        //        {
        //            Title = po.OptionName,
        //            Values = po.OptionValues.Select(v => v.Value).ToList(),
        //            Variants = po.OptionValues.Select(ov => new ProductOptionVariantDto
        //            {
        //                Title = ov.Value,
        //                Label = ov.Label,
        //                Images = ov.ProductImages!.Select(pi => new ImageLookupDto
        //                {
        //                    Id = pi.Id,
        //                    Url = pi.ImageUrl
        //                }).ToList()
        //            }).ToList()
        //        }).ToList(),
        //    });
    }
}
