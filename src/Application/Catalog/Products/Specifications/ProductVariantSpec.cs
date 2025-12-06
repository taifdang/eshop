using Application.Catalog.Products.Queries.GetVariantById;
using Application.Catalog.Products.Queries.GetVariantByOption;
using Application.Common.Models;
using Ardalis.Specification;
using Domain.Entities;

namespace Application.Catalog.Products.Specifications;

//public class ProductVariantSpec : Specification<Variant>
//{
//    public ProductVariantSpec()
//    {
//        Query.AsNoTracking();
//    }

//    public ProductVariantSpec ByProductId(Guid? productId)
//    {
//        if(productId.HasValue)
//            Query.Where(x => x.ProductId == productId);

//        return this;
//    }

//    public ProductVariantSpec ByVariantId(Guid variantId)
//    {
//        Query.Where(x => x.Id == variantId);

//        return this;
//    }

//    public ProductVariantSpec WithInStock()
//    {
//        Query.Where(x => x.Status == Domain.Enums.IntentoryStatus.InStock);

//        return this;
//    }

//    public ProductVariantSpec FilterByOptions(List<Guid> optionValues, bool exactMatch)
//    {
//        if (optionValues.Any())
//        {
//            // Not enough to option values, need to match all provided options
//            Query.Where(x => 
//                optionValues.All(opt => x.VariantOptionValues.Any(v => v.OptionValue.Id == opt)));
//            // Corrected to ensure exact match of option values
//            // Ex: Color : Red, Size: M  should not match Color: Red, Size: M, Material: Cotton
//            if (exactMatch)
//                Query.Where(x => x.VariantOptionValues.Count == optionValues.Count);
//        }

//        return this;
//    }
//}

//public class ProductVariantProjectionSpec : Specification<Variant, VariantVm>
//{
//    public ProductVariantProjectionSpec()
//    {
//        Query.Select(x => new VariantVm
//        {
//            Id = x.Id,
//            Title = x.Title ?? "",
//            ProductId = x.ProductId,
//            ProductName = x.Product.Name,
//            Price = x.Price,
//            Percent = x.Percent,
//            Quantity = x.Quantity,
//            Sku = x.Sku ?? "",
//            Image = null,
//            Options = x.VariantOptionValues.Select(y => new OptionLookupDto
//            {
//                Title = y.OptionValue.ProductOption.Name,
//                OptionValueId = y.OptionValueId,
//                Value = y.OptionValue.Value,
//                IsImage = y.OptionValue.ProductOption.AllowImage
//            }).ToList()
//        });
//    }
//}

//public class ProductVariantItemProjectionSpec : Specification<Variant, VariantItemDto>
//{
//    public ProductVariantItemProjectionSpec()
//    {
//        Query.Select(x => new VariantItemDto
//        {
//            Id = x.Id,
//            Price = x.Price,
//            Percent = x.Percent,
//            Quantity = x.Quantity,
//            Options = x.VariantOptionValues.Select(y => new OptionValueLookupDto
//            {
//                OptionId = y.OptionValue.ProductOption.Name,
//                OptionValueId = y.OptionValueId,
//                Value = y.OptionValue.Value
//            })
//             .OrderBy(o => o.Title)
//             .ToList()
//        });
//    }
//}

