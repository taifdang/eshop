using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Outbox.EF.Infrastructure.Data;

public class OutboxDbContextFactory : IDesignTimeDbContextFactory<OutboxDbContext>
{
    public OutboxDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OutboxDbContext>();
        optionsBuilder.UseSqlServer("Server=LAPTOP-J20BGGNG\\SQLEXPRESS;Database=ecommerce_db;Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=True");
        return new OutboxDbContext(optionsBuilder.Options);
    }
}
