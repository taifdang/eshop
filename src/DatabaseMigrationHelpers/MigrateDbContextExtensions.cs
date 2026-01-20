using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
namespace DatabaseMigrationHelpers;

public static class MigrateDbContextExtensions
{
    public const string ActitvitySourceName = "Migrations";
    private static readonly ActivitySource ActivitySource = new(ActitvitySourceName);

    public static async Task<IHost> MigrationDbContextAsync<TContext>(
        this IHost host,
        CancellationToken cancellationToken = default)
       where TContext : DbContext
    {
        await using var scope = host.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetService<TContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

        if (context is not null)
        {
            using var activity = ActivitySource.StartActivity($"Migrating {typeof(TContext).Name}", ActivityKind.Client);
            activity?.SetTag("migration", "efcore");

            try
            {
                await MigrationAsync(context, logger);

                var seeders = scope.ServiceProvider.GetServices<IDataSeeder<TContext>>();

                if(seeders is null || !seeders.Any())
                {
                    logger.LogInformation("No seeders found for DbContext {DbContext}", typeof(TContext).Name);
                    return host;
                }

                foreach (var seeder in seeders)
                {
                    logger.LogInformation("Seeding {Seeder} for DbContext {DbContext}", seeder.GetType().Name, typeof(TContext).Name);

                    await seeder.SeedAsync(cancellationToken);
                }

            }
            catch (Exception ex)
            {                
                activity?.AddException(ex);
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                throw;
            }
        }

        return host;
    }

    public static async Task MigrationAsync<TContext>(TContext context, ILogger<TContext> logger, CancellationToken cancellationToken = default)
       where TContext : DbContext
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync(cancellationToken);

            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());

                await context.Database.MigrateAsync(cancellationToken);

                logger.LogInformation("Database migrations applied successfully.");
            }
        });
    } 
}
