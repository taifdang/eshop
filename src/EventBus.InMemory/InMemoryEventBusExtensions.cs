using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

namespace EventBus.InMemory;

public static class InMemoryEventBusExtensions
{
    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services)
    {
        var options = new InMemoryEventBusOptions();
        var channel = Channel.CreateBounded<MessageEnvelope>(options.MaxQueue);

        services.AddSingleton(channel);
        services.AddSingleton<IEventPublisher, InMemoryEventBusSender>();
        services.AddHostedService<InMemoryEventBusReceiver>();

        return services;
    }
}
