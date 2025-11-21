using MassTransit;

namespace Infrastructure.Data.Seed;

public static class InitialData
{
    public static List<Domain.Entities.Category> Categories { get; set; }
    public static List<Domain.Entities.Product> Products { get; set; }
    public static List<Domain.Entities.ProductOption> ProductOptions { get; set; }
    public static List<Domain.Entities.OptionValue> OptionValues { get; set; }
    public static List<Domain.Entities.ProductImage> ProductImages { get; set; }
    public static List<Domain.Entities.ProductVariant> ProductVariants { get; set; }
    static InitialData()
    {

        Categories = new List<Domain.Entities.Category>
        {
           new Domain.Entities.Category { Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230c8"), Title = "Shirt", Label = "shirt"}
        };

        Products = new List<Domain.Entities.Product>
        {
            new Domain.Entities.Product 
            {
                Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230d8"), 
                Title = "T-Shirt Blu",
                Description = "Lorem ipsum 1",
                CategoryId = Categories.First().Id,              
                Status = Domain.Enums.ProductStatus.Published
            },
            new Domain.Entities.Product
            {
                Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230d9"),
                Title = "T-Shirt Coolman",
                Description = "Lorem ipsum 2",
                CategoryId = Categories.First().Id,
                Status = Domain.Enums.ProductStatus.Published,
            }
        };

        ProductOptions = new List<Domain.Entities.ProductOption>
        {
            new Domain.Entities.ProductOption
            {
               Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230e1"),
               ProductId = Products.Last().Id,
               OptionName = "Color",
               AllowImage = true,
            },
            new Domain.Entities.ProductOption
            {
               Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230e2"),
               ProductId = Products.Last().Id,
               OptionName = "Size",
               AllowImage = false,
            }
        };

        OptionValues = new List<Domain.Entities.OptionValue>
        {
            new Domain.Entities.OptionValue
            {
               Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f1"),
               ProductOptionId = ProductOptions.First().Id,
               Value = "Black"
            },
            new Domain.Entities.OptionValue
            {
               Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f2"),
               ProductOptionId = ProductOptions.First().Id,
               Value = "White"
            },
            new Domain.Entities.OptionValue
            {
               Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f3"),
               ProductOptionId = ProductOptions.Last().Id,
               Value = "S",
               Label = "s"
            },
            new Domain.Entities.OptionValue
            {
               Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f4"),
               ProductOptionId = ProductOptions.Last().Id,
               Value = "M",
               Label = "m"
            },
        };

        ProductImages = new List<Domain.Entities.ProductImage>
        {
            new Domain.Entities.ProductImage
            {
               Id = NewId.NextGuid(),
               ProductId = Products.First().Id,
               ImageUrl = "/image/brown2.jpg",
               IsMain = true,
            },
            new Domain.Entities.ProductImage
            {
               Id = NewId.NextGuid(),
               ProductId = Products.First().Id,
               ImageUrl = "/image/brown1.jpg",
               IsMain = false,
            },
            new Domain.Entities.ProductImage
            {
               Id = NewId.NextGuid(),
               ProductId = Products.Last().Id,
               ImageUrl = "/image/black1.jpg",
               IsMain = true,
            },
            new Domain.Entities.ProductImage
            {
               Id = NewId.NextGuid(),
               ProductId = Products.Last().Id,
               ImageUrl = "/image/white1.jpg",
               IsMain = false,
            },
            new Domain.Entities.ProductImage
            {
               Id = NewId.NextGuid(),
               ProductId = Products.Last().Id,
               OptionValueId = OptionValues.First(x => x.Value == "Black").Id,
               ImageUrl = "/image/black2.jpg",
               IsMain = false,
            },
            new Domain.Entities.ProductImage
            {
               Id = NewId.NextGuid(),
               ProductId = Products.Last().Id,
               OptionValueId = OptionValues.First(x => x.Value == "White").Id,
               ImageUrl = "/image/white2.jpg",
               IsMain = false,
            },
        };

        //ProductVariants = new List<Domain.Entities.ProductVariant>
        //{
        //    new Domain.Entities.ProductVariant
        //    {
        //        Id = 1,
        //        ProductId = 1,
        //        Price = 50000,
        //        Quantity = 10,
        //        Percent = 0,
        //        Status = 0,
        //    }
        //    // Product with Id 2 will seed with mediatr
        //};
    }
}
