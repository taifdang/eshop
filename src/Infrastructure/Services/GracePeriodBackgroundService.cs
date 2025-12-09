using Contracts.IntegrationEvents;
using EventBus.Abstractions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Constants;
using System.Data.Common;

namespace Infrastructure.Services;

public class GracePeriodBackgroundService(
    ILogger<GracePeriodBackgroundService> logger,
    IOptions<BackgroundTaskOptions> options,
    IServiceScopeFactory serviceScopeFactory,
    IEventPublisher eventPublisher
    ) : BackgroundService
{
    private readonly BackgroundTaskOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    //private readonly ApplicationDbContext _dbcontext = dbcontext;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IEventPublisher _eventPublisher = eventPublisher;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTime = TimeSpan.FromSeconds(_options.CheckUpdateTime);

        logger.LogInformation("GracePeriodBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await CheckConfirmedGracePeriodOrders(db, stoppingToken);
            await Task.Delay(delayTime, stoppingToken);
        }

        logger.LogInformation("GracePeriodBackgroundService is stopping");
    }

    private async Task CheckConfirmedGracePeriodOrders(ApplicationDbContext dbContext, CancellationToken ct)
    {
        var orderIds = await GetConfirmedGracePeriodOrders(dbContext, ct);
        foreach (var orderId in orderIds)
        {
            var @event = new GracePeriodConfirmedIntegrationEvent { OrderId = orderId };

            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.OrderId, @event);

            await _eventPublisher.PublishAsync(@event);
        }
    }

    private async ValueTask<List<Guid>> GetConfirmedGracePeriodOrders(ApplicationDbContext dbContext, CancellationToken ct)
    {
        try
        {
            // logic: set locked edit order = processing status
            var cutoff = DateTime.UtcNow - TimeSpan.FromMinutes(_options.GracePeriodTime);

            var ids = await dbContext.Orders
                .Where(x =>
                    x.OrderDate <= cutoff && 
                    x.Status == Domain.Enums.OrderStatus.Pending)
                .Select(x => x.Id)
                .ToListAsync(ct);

            return ids;
        }
        catch(DbException ex)
        {
            logger.LogError(ex, "Fatal error establishing database connection");
        }

        return [];
    }
}