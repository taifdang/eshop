using MassTransit;

namespace Infrastructure.Data.Seed;

public static class InitialData
{
    //public static List<Domain.Entities.Category> Categories { get; set; }
    //public static List<Domain.Entities.Product> Products { get; set; }
    //public static List<Domain.Entities.Option> ProductOptions { get; set; }
    //public static List<Domain.Entities.OptionValue> OptionValues { get; set; }
    //public static List<Domain.Entities.eImage> ProductImages { get; set; }
    //public static List<Domain.Entities.Variant> ProductVariants { get; set; }
    //static InitialData()
    //{

    //    Categories = new List<Domain.Entities.Category>
    //    {
    //       new Domain.Entities.Category { Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230c8"), Name = "Shirt", Label = "shirt"}
    //    };

    //    Products = new List<Domain.Entities.Product>
    //    {
    //        new Domain.Entities.Product 
    //        {
    //            Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230d8"), 
    //            Name = "T-Shirt Blu",
    //            Description = "Lorem ipsum 1",
    //            CategoryId = Categories.First().Id,              
    //            Status = Domain.Enums.ProductStatus.Published
    //        },
    //        new Domain.Entities.Product
    //        {
    //            Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230d9"),
    //            Name = "T-Shirt Coolman",
    //            Description = "Lorem ipsum 2",
    //            CategoryId = Categories.First().Id,
    //            Status = Domain.Enums.ProductStatus.Published,
    //        }
    //    };

    //    ProductOptions = new List<Domain.Entities.Option>
    //    {
    //        new Domain.Entities.Option
    //        {
    //           Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230e1"),
    //           ProductId = Products.Last().Id,
    //           Name = "Color",
    //           AllowImage = true,
    //        },
    //        new Domain.Entities.Option
    //        {
    //           Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230e2"),
    //           ProductId = Products.Last().Id,
    //           Name = "Size",
    //           AllowImage = false,
    //        }
    //    };

    //    OptionValues = new List<Domain.Entities.OptionValue>
    //    {
    //        new Domain.Entities.OptionValue
    //        {
    //           Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f1"),
    //           OptionId = ProductOptions.First().Id,
    //           Value = "Black"
    //        },
    //        new Domain.Entities.OptionValue
    //        {
    //           Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f2"),
    //           OptionId = ProductOptions.First().Id,
    //           Value = "White"
    //        },
    //        new Domain.Entities.OptionValue
    //        {
    //           Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f3"),
    //           OptionId = ProductOptions.Last().Id,
    //           Value = "S",
    //           Label = "s"
    //        },
    //        new Domain.Entities.OptionValue
    //        {
    //           Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230f4"),
    //           OptionId = ProductOptions.Last().Id,
    //           Value = "M",
    //           Label = "m"
    //        },
    //    };

    //    ProductImages = new List<Domain.Entities.eImage>
    //    {
    //        new Domain.Entities.Image
    //        {
    //           Id = NewId.NextGuid(),
    //           ProductId = Products.First().Id,
    //           Url = "/image/brown2.jpg",
    //           IsMain = true,
    //        },
    //        new Domain.Entities.Image
    //        {
    //           Id = NewId.NextGuid(),
    //           ProductId = Products.First().Id,
    //           Url = "/image/brown1.jpg",
    //           IsMain = false,
    //        },
    //        new Domain.Entities.Image
    //        {
    //           Id = NewId.NextGuid(),
    //           ProductId = Products.Last().Id,
    //           Url = "/image/black1.jpg",
    //           IsMain = true,
    //        },
    //        new Domain.Entities.Image
    //        {
    //           Id = NewId.NextGuid(),
    //           ProductId = Products.Last().Id,
    //           Url = "/image/white1.jpg",
    //           IsMain = false,
    //        },
    //        new Domain.Entities.Image
    //        {
    //           Id = NewId.NextGuid(),
    //           ProductId = Products.Last().Id,
    //           OptionValueId = OptionValues.First(x => x.Value == "Black").Id,
    //           Url = "/image/black2.jpg",
    //           IsMain = false,
    //        },
    //        new Domain.Entities.Image
    //        {
    //           Id = NewId.NextGuid(),
    //           ProductId = Products.Last().Id,
    //           OptionValueId = OptionValues.First(x => x.Value == "White").Id,
    //           Url = "/image/white2.jpg",
    //           IsMain = false,
    //        },
    //    };

        //Variants = new List<Domain.Entities.Variant>
        //{
        //    new Domain.Entities.Variant
        //    {
        //        OptionValueId = 1,
        //        ProductId = 1,
        //        Price = 50000,
        //        Quantity = 10,
        //        Percent = 0,
        //        Status = 0,
        //    }
        //    // Product with OptionValueId 2 will seed with mediatr
        //};
   // }
}
