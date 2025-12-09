using Microsoft.EntityFrameworkCore;
using Outbox.Abstractions;

namespace Outbox.EF.Infrastructure.Data;

public class OutboxDbContext : DbContext
{
    public OutboxDbContext(DbContextOptions<OutboxDbContext> options) : base(options)
    {
    }
    public DbSet<PollingOutboxMessage> PollingOutboxMessages => Set<PollingOutboxMessage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PollingOutboxMessage>()
            .HasIndex(nameof(PollingOutboxMessage.CreateDate), nameof(PollingOutboxMessage.RetryCount), nameof(PollingOutboxMessage.ProcessedDate));
    }
}
