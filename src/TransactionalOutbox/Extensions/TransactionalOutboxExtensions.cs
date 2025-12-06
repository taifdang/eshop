using EventBus.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransactionalOutbox.Abstractions;
using TransactionalOutbox.Infrastructure;
using TransactionalOutbox.Infrastructure.Data;
using TransactionalOutbox.Infrastructure.Service;

namespace TransactionalOutbox.Extensions;

public static class TransactionalOutboxExtensions
{
    public static void AddTransactionalOutbox(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OutboxDbContext>();
        builder.Services.AddScoped<IOutboxMessageProcessor, OutboxMessageProcessor>();
        builder.Services.AddScoped<IPollingOutboxMessageRepository, PollingOutboxMessageRepository>();
        builder.Services.AddSingleton<PollingOutboxMessageRepositoryOptions>();
        //builder.Services.AddTransient<IEventPublisher, NullEventPublisher>();
        builder.Services.AddInMemoryEventBus();
        //builder.Services.AddHostedService<TransactionalOutboxPollingService>();
    }
}
