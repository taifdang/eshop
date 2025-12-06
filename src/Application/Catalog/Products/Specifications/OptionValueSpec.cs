using Ardalis.Specification;
using Domain.Entities;

namespace Application.Catalog.Products.Specifications;

//public class OptionValueSpec : Specification<OptionValue>
//{
//    public OptionValueSpec()
//    {
//        Query.AsNoTracking();
//    }

    //public OptionValueSpec FilterByIds(Guid? optionValueId, Guid optionId)
    //{
    //    Query
    //       .Include(x => x.Option)
    //       .Where(x =>
    //       (!optionValueId.HasValue || x.OptionValueId == optionValueId) &&
    //       x.Option.OptionValueId == optionId);

    //    return this;
    //}

    //public OptionValueSpec ByOptionValueId(Guid? optionValueId)
    //{
    //    if(optionValueId.HasValue)
    //        Query.Where(x => x.OptionValueId == optionValueId);
       
    //    return this;
    //}

    //public OptionValueSpec ByOptionId(Guid optionId)
    //{
    //    Query.Where(x => x.ProductOption.OptionValueId == optionId);

    //    return this;
    //}

    //public OptionValueSpec ByProductId(Guid productId)
    //{
    //    Query.Where(x => x.ProductOption.ProductId == productId);

    //    return this;
    //}

    //public OptionValueSpec WithOptionValues(List<Guid> optionValues)
    //{
    //    Query.Where(x => optionValues.Contains(x.OptionValueId));

    //    return this;
    //}

    //public OptionValueSpec AllowImage()
    //{
    //    Query.Where(x => x.ProductOption.AllowImage);

    //    return this;
    //}

    //public OptionValueSpec ImageAllowedWithOptionValue(Guid optionValueId, Guid productId)
    //{
    //    Query
    //        .Where(x =>
    //            x.Option.ProductId == productId &&
    //            x.OptionValueId == optionValueId &&
    //            x.Option.AllowImage);

    //    return this;
    //}

//}

//public class OptionValueProjectionSpec : Specification<OptionValue, OptionValueDto>
//{
//    public OptionValueProjectionSpec()
//    {
//        Query.Select(x => new OptionValueDto(x.OptionValueId, x.Value, x.Label));
//    }
//}
