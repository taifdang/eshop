using Application.Common.Interfaces.Eventbus;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace Infrastructure.Messaging.InMemory;

public class InMemorySender<T> : IEventBusSender<T>
{
    private readonly Channel<T> _channel;

    public InMemorySender(IOptions<InMemoryOptions> options)
    {
        _channel = Channel.CreateBounded<T>(options.Value.Capacity);
    }

    public async Task SendAsync(T @event, CancellationToken cancellationToken = default)
    {
        await _channel.Writer.WriteAsync(@event, cancellationToken);
    }
}
