using Application.Common.Interfaces.Eventbus;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.HostServices;

public sealed class MessageBusConsumerBackgroundService<TConsumer, T> : BackgroundService
{
    private readonly IEventBusReceiver<TConsumer, T> _receiver;
    private readonly IIntegrationEventHandler<T> _handler;

    public MessageBusConsumerBackgroundService(
        IEventBusReceiver<TConsumer, T> receiver, 
        IIntegrationEventHandler<T> handler)
    {
        _receiver = receiver;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _receiver.ReceiveAsync(_handler.HandleAsync, stoppingToken);
    }
}
