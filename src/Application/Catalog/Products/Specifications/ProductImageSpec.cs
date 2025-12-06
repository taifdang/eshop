using Application.Common.Models;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Catalog.Products.Specifications;

//public class ProductImageSpec : Specification<ProductImage>
//{
//    public ProductImageSpec()
//    {
//        Query.AsNoTracking();
//    }

    //public ProductImageSpec ByProductId(Guid ProductId)
    //{
    //    Query.Where(x => x.ProductId == ProductId);
    //    return this;
    //}

    //public ProductImageSpec ById(Guid OptionValueId)
    //{
    //    Query.Where(x => x.OptionValueId == OptionValueId);
    //    return this;
    //}
    //public ProductImageSpec ByOptionValueId(Guid? OptionValueId)
    //{
    //    if (OptionValueId.HasValue)
    //        Query.Where(x => x.OptionValueId == OptionValueId.Value);
    //    return this;
    //}

    //public ProductImageSpec ApplyOrderingBy(Guid? OptionValueId)
    //{
    //    Query
    //         .OrderBy(x =>
    //              x.OptionValueId == OptionValueId ? 0 :
    //              x.IsMain && x.OptionValueId == null ? 1 :
    //              x.OptionValueId == null ? 2 : 3)
    //        .ThenBy(x => x.OptionValueId);
    //    return this;
    //}

    //public ProductImageSpec TakeOne()
    //{
    //    Query.Take(1);
    //    return this;
    //}

    //public ProductImageSpec ProjectToLookupDto()
    //{
    //    Query.Select(x => new ImageWithOptionValueLookupDto
    //    {
    //        LookupDto = new ImageLookupDto
    //        {
    //            OptionValueId = x.OptionValueId,
    //            Url = x.Url
    //        },
    //        OptionValueId = x.OptionValueId
    //    });

    //    return this;
    //}
//}

//public class ProductImageProjectionSpec : Specification<ProductImage, ImageWithOptionValueLookupDto>
//{
//    public ProductImageProjectionSpec()
//    {
//        Query.Select(x => new ImageWithOptionValueLookupDto
//        {
//            LookupDto = new ImageLookupDto
//            {
//                OptionValueId = x.OptionValueId,
//                Url = x.Url
//            },
//            OptionValueId = x.OptionValueId
//        });
//    }
//}
