using Application.Catalog.Products.Queries.GetListProduct;
using Application.Common.Models;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Common.Specifications;

public class ProductListPaginationSpec : Specification<Product, ProductListDto>
{
    public ProductListPaginationSpec(int skip, int take)
        : base()
    {
        if(take == 0)
        {
            take = int.MaxValue;
        }

        Query.Where(x => x.Status == Domain.Enums.ProductStatus.Published).AsNoTracking();

        Query.OrderBy(x => x.Id).Skip(skip).Take(take);

        Query.Select(x => new ProductListDto
        {
            Id = x.Id,
            Title = x.Title,
            Price = x.ProductVariants.Min(pv => pv.Price),
            Category = x.Category.Title,
            Image = x.ProductImages
                    .Where(c => c.IsMain && c.OptionValueId == null) // main image
                    .Select(pi => new ImageLookupDto
                    {
                        Id = pi.Id,
                        Url = pi.ImageUrl
                    })
                    .FirstOrDefault() ?? new(),
        });
    }
}
