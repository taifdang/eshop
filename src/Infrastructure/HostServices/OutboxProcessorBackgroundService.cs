using Application.Common.Interfaces.Eventbus;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using System.Text.Json;

namespace Infrastructure.HostServices;

public class OutboxProcessorBackgroundService 
{
    private readonly EfRepository<OutBoxMessage> _outboxRepository;
    private readonly ApplicationDbContext _context;
    private readonly IEventBus _eventBus;

    public OutboxProcessorBackgroundService(
        EfRepository<OutBoxMessage> outboxRepository,
        ApplicationDbContext context,
        IEventBus eventBus)
    {
        _outboxRepository = outboxRepository;
        _context = context;
        _eventBus = eventBus;
    }

    protected  async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var events = _context.OutBoxMessages
            .Where(e => !e.IsPublished)
            .OrderBy(e => e.LastModified)
            .Take(50)
            .ToList();

        foreach (var @event in events)
        {
            var outbox = new OutboxMessageData
            {
                Id = @event.Id.ToString(),
                EventType = @event.EventType,
                Payload = @event.Payload,
            };

            var type = Type.GetType(@event.EventType);
            var message = JsonSerializer.Deserialize(outbox.Payload, type!);

            await _eventBus.SendAsync(message, stoppingToken);

            @event.IsPublished = true;  
            @event.LastModified = DateTime.UtcNow;
            await _outboxRepository.UnitOfWork.SaveChangesAsync(stoppingToken);
        }
    }
}
