using Ardalis.Specification;
using Domain.Entities;

namespace Application.Catalog.Products.Specifications;

//public class ProductOptionSpec : Specification<Option>
//{
//    public ProductOptionSpec()
//    {
//        Query.AsNoTracking();
//    }

//    public ProductOptionSpec ByProductId(Guid productId)
//    {
//        Query.Where(x => x.ProductId == productId);

//        return this;
//    }

//    public ProductOptionSpec ByOptionId(Guid? optionId)
//    {
//        if(optionId.HasValue)
//            Query.Where(x => x.Id == optionId.Value);

//        return this;
//    }

//    public ProductOptionSpec AllowImage()
//    {
//        Query.Where(x => x.AllowImage);

//        return this;
//    }
//}
