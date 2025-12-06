using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace TransactionalOutbox.Infrastructure.Data;

public class OutboxDbDesignTimeDbContextFactory : IDesignTimeDbContextFactory<OutboxDbContext>
{
    public OutboxDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OutboxDbContext>();
        optionsBuilder.UseSqlServer("Server=LAPTOP-J20BGGNG\\SQLEXPRESS;Database=ecommerce_db;Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=True");
        return new OutboxDbContext(optionsBuilder.Options);
    }
}
