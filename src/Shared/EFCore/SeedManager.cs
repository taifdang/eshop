using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Shared.EFCore;

public class SeedManager(
    ILogger<SeedManager> logger,
    IWebHostEnvironment env,
    IServiceProvider serviceProvider) : ISeedManager
{
    public async Task ExecuteSeedAsync()
    {
        if (!env.IsEnvironment("test"))
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var dataSeeders = scope.ServiceProvider.GetServices<IDataSeeder>();

            foreach (var seeder in dataSeeders)
            {
                logger.LogInformation("Seed {SeederName} is stared.", seeder.GetType().Name);
                await seeder.SendAllAsync();
                logger.LogInformation("Seed {SeederName} is stared.", seeder.GetType().Name);
            }
        }
    }
}
