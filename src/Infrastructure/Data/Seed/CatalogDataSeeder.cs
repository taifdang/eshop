using Application.Catalog.Variants.Commands.GenerateVariant;
using Application.Catalog.Variants.Commands.UpdateVariants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.EFCore;

namespace Infrastructure.Data.Seed;

public class CatalogDataSeeder : IDataSeeder
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public CatalogDataSeeder(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    public async Task SendAllAsync()
    {
        var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
        if (!pendingMigrations.Any())
        {
            await SeedCategories();
            await SeedProducts();
            await SeedProductOptions();
            await SeedOptionValues();
            await SeedProductImages();
            await SeedProductVariants();
        }
    }

    private async Task SeedCategories()
    {
        if (!_dbContext.Categories.Any())
        {
            await _dbContext.Categories.AddRangeAsync(InitialData.Categories);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task SeedProducts()
    {
        if (!_dbContext.Products.Any())
        {
            await _dbContext.Products.AddRangeAsync(InitialData.Products);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task SeedProductOptions()
    {
        if (!_dbContext.ProductOptions.Any())
        {
            await _dbContext.ProductOptions.AddRangeAsync(InitialData.ProductOptions);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task SeedOptionValues()
    {
        if (!_dbContext.OptionValues.Any())
        {
            await _dbContext.OptionValues.AddRangeAsync(InitialData.OptionValues);
            await _dbContext.SaveChangesAsync();
        }
    }
    public async Task SeedProductImages()
    {
        if (!_dbContext.ProductImages.Any())
        {
            await _dbContext.ProductImages.AddRangeAsync(InitialData.ProductImages);
            await _dbContext.SaveChangesAsync();
        }
    }
    private async Task SeedProductVariants()
    {
        if (!_dbContext.ProductVariants.Any())
        {
            foreach (var product in InitialData.Products)
            {
                var optionValueFilter = InitialData.ProductOptions
                    .Where(po => po.ProductId == product.Id)
                    .ToDictionary(
                        po => po.Id,
                        po => InitialData.OptionValues
                                .Where(ov => ov.ProductOptionId == po.Id)
                                .Select(ov => ov.Id)
                                .ToList()
                    );

                if (!optionValueFilter.Any())
                {
                    // No Option
                    await _dbContext.ProductVariants.AddAsync(new Domain.Entities.ProductVariant
                    {
                        ProductId = product.Id,
                        Price = 40000,
                        Quantity = 20,
                        //Sku = $"SKU-{product.Id}-1"
                    });
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    // Have Option
                    var generateCommand = new GenerateVariantCommand(product.Id, optionValueFilter);

                    await _mediator.Send(generateCommand);

                    var updateCommand = new UpdateVariantsCommand(product.Id, 40000, 20, null);

                    await _mediator.Send(updateCommand);
                }
            }
        }          
    }
}