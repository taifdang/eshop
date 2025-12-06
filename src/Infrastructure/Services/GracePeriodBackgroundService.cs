using Contracts.IntegrationEvents;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Constants;
using System.Data.Common;

namespace Infrastructure.Services;

public class GracePeriodBackgroundService(
    ILogger<GracePeriodBackgroundService> logger,
    IOptions<BackgroundTaskOptions> options,
    ApplicationDbContext dbcontext) : BackgroundService
{
    private readonly BackgroundTaskOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    private readonly ApplicationDbContext _dbcontext = dbcontext;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var delayTime = TimeSpan.FromSeconds(_options.CheckUpdateTime);

        logger.LogInformation("GracePeriodBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckConfirmedGracePeriodOrders();
            await Task.Delay(delayTime, stoppingToken);
        }

        logger.LogInformation("GracePeriodBackgroundService is stopping");
    }

    private async Task CheckConfirmedGracePeriodOrders()
    {
        var orderIds = await GetConfirmedGracePeriodOrders();
        foreach (var orderId in orderIds)
        {
            var @event = new GracePeriodConfirmedIntegrationEvent { OrderId = orderId };

            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.OrderId, @event);

            await dbcontext.OutboxPollingRepository.SaveChangesAsync();
        }
    }

    private async ValueTask<List<Guid>> GetConfirmedGracePeriodOrders()
    {
        try
        {
            var ids = await _dbcontext.Set<Order>()
                .Where(x =>
                    DateTime.UtcNow - x.OrderDate >= TimeSpan.FromMinutes(_options.GracePeriodTime) && 
                    x.Status == Domain.Enums.OrderStatus.Submitted)
                .Select(x => x.Id)
                .ToListAsync();

            return ids;
        }
        catch(DbException ex)
        {
            logger.LogError(ex, "Fatal error establishing database connection");
        }

        return [];
    }
}