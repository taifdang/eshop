using EventBus.Abstractions;
using EventBus.Events;
using System.Text.Json;
using System.Threading.Channels;

namespace EventBus.InMemory;

public class InMemoryEventBusSender : IEventPublisher
{
    private readonly Channel<MessageEnvelope> _channel;

    public InMemoryEventBusSender(Channel<MessageEnvelope> channel)
    {
        _channel = channel;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent
    {
        var json = JsonSerializer.Serialize(@event, @event.GetType());

        var message = new MessageEnvelope(@event.GetType(), json);

        await _channel.Writer.WriteAsync(message);
    }
}
