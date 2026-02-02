using Microsoft.EntityFrameworkCore;

namespace Migrator;

public interface IDataSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(CancellationToken cancellationToken);
}
