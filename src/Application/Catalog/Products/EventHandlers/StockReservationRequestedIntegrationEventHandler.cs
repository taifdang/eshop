using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using Contracts.IntegrationEvents;
using Domain.Events;
using EventBus.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TransactionalOutbox.Abstractions;

namespace Application.Catalog.Products.EventHandlers;

public class StockReservationRequestedIntegrationEventHandler
    : IIntegrationEventHandler<StockReservationRequestedIntegrationEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IPollingOutboxMessageRepository _pollingOutboxMessageRepository;
    private readonly ILogger<StockReservationRequestedIntegrationEventHandler> _logger;

    public StockReservationRequestedIntegrationEventHandler(
        IApplicationDbContext context,
        IPollingOutboxMessageRepository pollingOutboxMessageRepository,
        ILogger<StockReservationRequestedIntegrationEventHandler> logger)
    {
        _context = context;
        _pollingOutboxMessageRepository = pollingOutboxMessageRepository;
        _logger = logger;
    }

    public async Task Handle(StockReservationRequestedIntegrationEvent request)
    {
        Guard.Against.Null(request);

        var failItems = new List<StockItems>();

        foreach (var item in request.Items)
        {
            var variant = await _context.Variants
                .Where(v => v.Id == item.VariantId)
                .FirstOrDefaultAsync();

            if (variant is null || variant.Quantity < item.Quantity)
            {
                failItems.Add(new StockItems(item.VariantId, item.Quantity, 0));
            }

            else
            {
                variant.ReduceStock(item.Quantity);
            }
        }

        if (failItems.Any())
        {
            _logger.LogWarning("Stock reservation rejected for OrderId: {OrderId}", request.OrderId);

            var integrationEvent = new ReserveStockRejectedIntegrationEvent
            {
                OrderId = request.OrderId
            };

            await _pollingOutboxMessageRepository.AddAsync(new PollingOutboxMessage
            {
                CreateDate = DateTime.UtcNow,
                PayloadType = typeof(ReserveStockRejectedIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
                Payload = JsonSerializer.Serialize(integrationEvent),
                ProcessedDate = null
            });

            await _pollingOutboxMessageRepository.SaveChangesAsync();
            return; // Exit early if reservation rejected
        }

        try
        {
            var integrationEvent = new ReserveStockSucceededIntegrationEvent
            {
                OrderId = request.OrderId
            };

            await _pollingOutboxMessageRepository.AddAsync(new PollingOutboxMessage
            {
                CreateDate = DateTime.UtcNow,
                PayloadType = typeof(ReserveStockSucceededIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
                Payload = JsonSerializer.Serialize(integrationEvent),
                ProcessedDate = null
            });

            await _context.SaveChangesAsync();
            await _pollingOutboxMessageRepository.SaveChangesAsync();     
            
            _logger.LogInformation("Stock reserved successfully for OrderId: {OrderId}", request.OrderId);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("Concurrency conflict");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reserving stock for OrderId: {OrderId}", request.OrderId);

            var integrationEvent = new ReserveStockFailedIntegrationEvent
            {
                OrderId = request.OrderId
            };

            await _pollingOutboxMessageRepository.AddAsync(new PollingOutboxMessage
            {
                CreateDate = DateTime.UtcNow,
                PayloadType = typeof(ReserveStockSucceededIntegrationEvent).FullName ?? throw new Exception($"Could not get fullname of type {integrationEvent.GetType()}"),
                Payload = JsonSerializer.Serialize(integrationEvent),
                ProcessedDate = null
            });

            await _pollingOutboxMessageRepository.SaveChangesAsync();
        }
    }
}
