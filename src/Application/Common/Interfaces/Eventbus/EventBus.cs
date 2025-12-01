using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Interfaces.Eventbus;

public class EventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;

    public EventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        await _serviceProvider.GetRequiredService<IEventBusSender<T>>().SendAsync(message, cancellationToken);
    }
}
