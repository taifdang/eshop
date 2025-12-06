using Application.Catalog.Products.Queries.GetListProduct;
using Application.Catalog.Products.Queries.GetProductById;
using Application.Common.Models;
using Ardalis.Specification;

namespace Application.Catalog.Products.Specifications;

//public class ProductSpec : Specification<Domain.Entities.Product>
//{
//    public ProductSpec()
//    {
        
//    }

//    public ProductSpec ById(Guid productId)
//    {
//        Query.Where(x => x.OptionValueId == productId);

//        return this;
//    }

//    public ProductSpec WithIsPublished()
//    {
//        Query.Where(x => x.Status == Domain.Enums.ProductStatus.Published);

//        return this;
//    }

//    public ProductSpec ApplyPaging(int skip, int take)
//    {
//        if (take == 0)
//        {
//            take = int.MaxValue;
//        }

//        Query.OrderBy(x => x.OptionValueId).Skip(skip).Take(take);

//        return this;
//    }
//}

//public class ProductListProjectionSpec : Specification<Domain.Entities.Product, ProductListDto>
//{
//    public ProductListProjectionSpec()
//    {
//        Query.Select(x => new ProductListDto
//        {
//            OptionValueId = x.OptionValueId,
//            OptionId = x.Name,
//            Price = x.Variants.Min(pv => pv.Price),
//            Category = x.Category.Name,
//            Image = x.Images
//                 .Where(c => c.IsMain && c.OptionValueId == null) // main image
//                 .Select(pi => new ImageLookupDto
//                 {
//                     OptionValueId = pi.OptionValueId,
//                     Url = pi.Url
//                 })
//                 .FirstOrDefault() ?? new(),
//        });
//    }
//}

//public class ProductItemProjectionSpec : Specification<Domain.Entities.Product, ProductItemDto>
//{
//    public ProductItemProjectionSpec()
//    {
//        Query.Select(p => new ProductItemDto
//        {
//            OptionValueId = p.OptionValueId,
//            OptionId = p.Name,
//            MinPrice = p.Variants.Min(pv => pv.Price),
//            MaxPrice = p.Variants.Max(pv => pv.Price),
//            Description = p.Description ?? "",
//            Category = p.Category.Name,
//            MainImage = null,
//            Images = null,
//            Options = p.Options.Select(po => new ProductOptionDto
//            {
//                OptionId = po.Name,
//                OptionValues = po.Values.Select(ov => new ProductOptionValueDto
//                {
//                    OptionValueId = ov.OptionValueId,
//                    Value = ov.Value,
//                    Image = null
//                }).ToList()
//            }).ToList()
//        });
//    }
//}


