using EventBus.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.Abstractions;
using Outbox.EF.Infrastructure;
using Outbox.EF.Infrastructure.Data;
using Outbox.EF.Infrastructure.Services;

namespace Outbox.EF.Extensions;

public static class OutboxEfExtensions
{
    public static void AddTransactionalOutbox(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OutboxDbContext>((sp, options) =>
        {
            options.UseSqlServer("Server=LAPTOP-J20BGGNG\\SQLEXPRESS;Database=ecommerce_db;Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=True");
        });
        builder.Services.AddScoped<IOutboxMessageProcessor, OutboxMessageProcessor>();
        builder.Services.AddScoped<IPollingOutboxMessageRepository, PollingOutboxMessageRepository>();
        builder.Services.AddSingleton<PollingOutboxMessageRepositoryOptions>();
        //builder.Services.AddTransient<IEventPublisher, NullEventPublisher>();
        builder.AddInMemoryEventBus();
        builder.Services.AddHostedService<TransactionalOutboxPollingService>();
    }
}

