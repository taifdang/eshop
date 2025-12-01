using Application.Common.Interfaces.Eventbus;
using System.Threading.Channels;

namespace Infrastructure.Messaging.InMemory;

public class InMemoryReceiver<TConsumer, T> : IEventBusReceiver<TConsumer, T>
{
    private readonly Channel<T> _channel;
    public InMemoryReceiver(Channel<T> channel)
    {
        _channel = channel;
    }
    public async Task ReceiveAsync(Func<T, CancellationToken, Task> action, CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = await _channel.Reader.ReadAsync(cancellationToken);

                await action(message, cancellationToken);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
    }
}
