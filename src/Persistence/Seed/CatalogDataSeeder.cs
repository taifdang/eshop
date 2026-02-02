using Migrator;

namespace Persistence.Seed;

public class CatalogDataSeeder : IDataSeeder<ApplicationDbContext>
{
    private readonly ApplicationDbContext _dbContext;

    public CatalogDataSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        await SeedCategories();
        await SeedProducts();
        await SeedProductOptions();
        await SeedOptionValues();
        await SeedProductImages();
        await SeedVariants();
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
        if (!_dbContext.ProducOptions.Any())
        {
            await _dbContext.ProducOptions.AddRangeAsync(InitialData.ProductOptions);
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
    private async Task SeedVariants()
    {
        if (!_dbContext.Variants.Any())
        {
            await _dbContext.Variants.AddRangeAsync(InitialData.Variants);
            await _dbContext.SaveChangesAsync();
        }
    }
}
