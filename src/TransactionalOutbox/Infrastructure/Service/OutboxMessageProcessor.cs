using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TransactionalOutbox.Abstractions;

namespace TransactionalOutbox.Infrastructure.Service;

public class OutboxMessageProcessor : IOutboxMessageProcessor
{
    private readonly ILogger<OutboxMessageProcessor> _logger;
    private readonly IEventPublisher _eventPublisher;
    private readonly IPollingOutboxMessageRepository _repository;

    public OutboxMessageProcessor(
        ILogger<OutboxMessageProcessor> logger, 
        IEventPublisher eventPublisher, 
        IPollingOutboxMessageRepository repository)
    {
        _logger = logger;
        _eventPublisher = eventPublisher;
        _repository = repository;
    }

    public async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var messages = await _repository.GetUnprocessedMessagesAsync();

            foreach (var message in messages)
            {
                try
                {
                    var @event = DeserializeJsonContent(message);

                    if (@event == null)
                    {
                        _logger.LogWarning("Failed to rebuild event from message {MessageId}", message.Id);
                        await _repository.MarkEventAsFailedAsync(message, false); // mark as failed without retry
                        continue;
                    }

                    _logger.LogInformation("Publish event from Polling publisher {m}", message.Payload);

                    await _eventPublisher.PublishAsync(@event);
                    await _repository.MarkEventAsProcessedAsync(message);
                    await _repository.SaveChangesAsync();

                    _logger.LogInformation("Published message {MessageId}", message.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to publish message {MessageId}", message.Id);
                }

                if (!messages.Any() && !cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, cancellationToken); // wait before next polling
                }
            }
        }
    }

    public static Type? GetFirstMatchingTypeFromCurrentDomainAssembly(string typeName)
    {
        var result = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName || x.Name == typeName))
            .FirstOrDefault();

        return result;
    }

    private IntegrationEvent? DeserializeJsonContent(PollingOutboxMessage message)
    {
        // here, not really safe, but for demo purposes it's ok
        var type = GetFirstMatchingTypeFromCurrentDomainAssembly(message.PayloadType);

        if (type == null)
        {
            _logger.LogWarning("Failed to find type {PayloadType}", message.PayloadType);
            return null;
        }

        return JsonSerializer.Deserialize(message.Payload, type) as IntegrationEvent;
    }
}
