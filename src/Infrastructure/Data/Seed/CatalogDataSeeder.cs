using Application.Catalog.Products.Commands.GenerateVariant;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.EFCore;

namespace Infrastructure.Data.Seed;

public class CatalogDataSeeder : IDataSeeder
{

    public Task SendAllAsync()
    {
        throw new NotImplementedException();
    }
    //private readonly IMediator _mediator;
    //private readonly ApplicationDbContext _dbContext;

    //public CatalogDataSeeder(IMediator mediator, ApplicationDbContext dbContext)
    //{
    //    _mediator = mediator;
    //    _dbContext = dbContext;
    //}

    //public async Task SendAllAsync()
    //{
    //    var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
    //    if (!pendingMigrations.Any())
    //    {
    //        await SeedCategories();
    //        await SeedProducts();
    //        //await SeedProductOptions();
    //        await SeedOptionValues();
    //        //await SeedProductImages();
    //        await SeedProductVariants();
    //    }
    //}

    //private async Task SeedCategories()
    //{
    //    if (!_dbContext.Categories.Any())
    //    {
    //        await _dbContext.Categories.AddRangeAsync(InitialData.Categories);
    //        await _dbContext.SaveChangesAsync();
    //    }
    //}
    //public async Task SeedProducts()
    //{
    //    if (!_dbContext.Products.Any())
    //    {
    //        await _dbContext.Products.AddRangeAsync(InitialData.Products);
    //        await _dbContext.SaveChangesAsync();
    //    }
    //}
    ////public async Task SeedProductOptions()
    ////{
    ////    if (!_dbContext.ProducOptions.Any())
    ////    {
    ////        await _dbContext.ProducOptions.AddRangeAsync(InitialData.ProductOptions);
    ////        await _dbContext.SaveChangesAsync();
    ////    }
    ////}
    //public async Task SeedOptionValues()
    //{
    //    if (!_dbContext.OptionValues.Any())
    //    {
    //        await _dbContext.OptionValues.AddRangeAsync(InitialData.OptionValues);
    //        await _dbContext.SaveChangesAsync();
    //    }
    //}
    ////public async Task SeedProductImages()
    ////{
    ////    if (!_dbContext.Images.Any())
    ////    {
    ////        await _dbContext.Images.AddRangeAsync(InitialData.ProductImages);
    ////        await _dbContext.SaveChangesAsync();
    ////    }
    ////}
    //private async Task SeedProductVariants()
    //{
    //    if (!_dbContext.Variants.Any())
    //    {
    //        foreach (var product in InitialData.Products)
    //        {
    //            var optionValueFilter = InitialData.ProductOptions
    //                .Where(po => po.ProductId == product.Id)
    //                .ToDictionary(
    //                    po => po.Id,
    //                    po => InitialData.OptionValues
    //                            .Where(ov => ov.OptionId == po.Id)
    //                            .Select(ov => ov.Id)
    //                            .ToList()
    //                );

    //            if (!optionValueFilter.Any())
    //            {
    //                // No Option
    //                await _dbContext.Variants.AddAsync(new Domain.Entities.Variant
    //                {
    //                    ProductId = product.Id,
    //                    Price = 40000,
    //                    Quantity = 20,
    //                    //Sku = $"SKU-{product.OptionValueId}-1"
    //                });
    //                await _dbContext.SaveChangesAsync();
    //            }
    //            else
    //            {
    //                // Have Option
    //                //var generateCommand = new GenerateVariantCommand(product.Id, optionValueFilter);

    //                //await _mediator.Send(generateCommand);

    //                //var updateCommand = new UpdateVariantsCommand(product.Id, 40000, 20, null);

    //                //await _mediator.Send(updateCommand);
    //            }
    //        }
    //    }          
    //}
}