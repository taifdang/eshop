using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;

namespace EventBus.InMemory;

public static class InMemoryEventBusExtensions
{
    public static IEventBusBuilder AddInMemoryEventBus(this IHostApplicationBuilder builder)
    {
        var options = new InMemoryEventBusOptions();
        var channel = Channel.CreateBounded<MessageEnvelope>(options.MaxQueue);

        builder.Services.AddSingleton(channel);
        builder.Services.AddSingleton(options);
        builder.Services.AddTransient<IEventPublisher, InMemoryEventBusSender>();
        builder.Services.AddHostedService<InMemoryEventBusReceiver>();

        return new InMemoryEventBusBuilder(builder.Services);
    }
    private class InMemoryEventBusBuilder(IServiceCollection services) : IEventBusBuilder
    {
        public IServiceCollection Services => services;
    }
}
