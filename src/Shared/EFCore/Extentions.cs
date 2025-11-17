using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Web;

namespace Shared.EFCore;

public static class Extentions
{
    public static IServiceCollection AddCustomDbContext<TContext>(this WebApplicationBuilder builder)
        where TContext : DbContext
    {
        builder.Services.AddValidateOptions<ConnectionStrings>();

        builder.Services.AddDbContext<TContext>(
            (sp, options) =>
            {
                string? connectionString = 
                    sp.GetRequiredService<ConnectionStrings>().DefaultConnection
                    ?? builder.Configuration?.GetSection("ConnectionStrings:DefaultConnection").ToString();

                ArgumentException.ThrowIfNullOrEmpty(connectionString);

                options.UseSqlServer(connectionString, dbOptions =>
                {
                    dbOptions.MigrationsAssembly(typeof(TContext).Assembly.GetName().Name);
                });

            });

        builder.Services.AddScoped<ISeedManager, SeedManager>();

        return builder.Services;
    }

    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
        where TContext : DbContext
    {
    
        MigrationAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

        SeedAsync(app.ApplicationServices).GetAwaiter().GetResult();

        return app;
    }

    private static async Task MigrationAsync<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>(); 

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        if(pendingMigrations.Any())
        {
            logger.LogInformation("Applying {Count} pending migrations. ",pendingMigrations.Count());

            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");
        }
    }

    private static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var seedersManager = scope.ServiceProvider.GetRequiredService<ISeedManager>();

        await seedersManager.ExecuteSeedAsync();
    }
}
