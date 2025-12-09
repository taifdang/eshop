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

    //public ProductImageSpec ById(Guid Id)
    //{
    //    Query.Where(x => x.Id == Id);
    //    return this;
    //}
    //public ProductImageSpec ByOptionValueId(Guid? Id)
    //{
    //    if (Id.HasValue)
    //        Query.Where(x => x.Id == Id.Value);
    //    return this;
    //}

    //public ProductImageSpec ApplyOrderingBy(Guid? Id)
    //{
    //    Query
    //         .OrderBy(x =>
    //              x.Id == Id ? 0 :
    //              x.IsMain && x.Id == null ? 1 :
    //              x.Id == null ? 2 : 3)
    //        .ThenBy(x => x.Id);
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
    //            Id = x.Id,
    //            Url = x.Url
    //        },
    //        Id = x.Id
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
//                Id = x.Id,
//                Url = x.Url
//            },
//            Id = x.Id
//        });
//    }
//}
