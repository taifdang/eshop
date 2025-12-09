using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Channels;

namespace EventBus.InMemory;

public class InMemoryEventBusReceiver(
    IServiceProvider serviceProvider,
    Channel<MessageEnvelope> channel,
    IOptions<EventBusSubscriptionManager> subscriptionManager,
    ILogger<InMemoryEventBusReceiver> logger) 
    : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly Channel<MessageEnvelope> _channel = channel;
    private readonly EventBusSubscriptionManager _subscriptions = subscriptionManager.Value;
    private readonly ILogger<InMemoryEventBusReceiver> _logger = logger;
  
    public async Task ProcessEvent<T>(T envelope) where T : MessageEnvelope
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        if (!_subscriptions.EventTypes.TryGetValue(envelope.MessageTypeName, out var eventType))
        {
            _logger.LogWarning("No subscription for event: {EventName}", envelope.MessageTypeName);
            return;
        }

        // deserialize the event
        var integrationEvent = JsonSerializer.Deserialize(envelope.Message, eventType, _subscriptions.JsonOptions) as IntegrationEvent;

        if(integrationEvent is null)
        {
            _logger.LogError("Deserialize event failed: {EventName}", envelope.MessageTypeName);
        }

        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            try
            {
                await handler.Handle(integrationEvent!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event: {EventName}", envelope.MessageTypeName);
            }
        }

#if (genericType)
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
        var handlers = scope.ServiceProvider.GetServices(handlerType);
        foreach (var handler in handlers)
        {
            try
            {
                await ((IIntegrationEventHandler)handler).Handle(integrationEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event: {EventName}", envelope.MessageTypeName);
            }
        }
#endif
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var message in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            if (message is MessageEnvelope envelope)
            {
                await ProcessEvent(envelope);
            }
            else 
            {
                _logger.LogWarning("Unknown message type: {Type}", message.GetType());
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
