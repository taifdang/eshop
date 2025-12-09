using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace EventBus;

public static class EventBusExtensions
{   
    public static IEventBusBuilder Configure(this IEventBusBuilder builder, Action<EventBusSubscriptionManager> configure)
    {
        //builder.Services.Configure<EventBusSubscriptionManager>(configure);

        builder.Services.PostConfigure<EventBusSubscriptionManager>(opt =>
        {
            opt.JsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.JsonOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });

        return builder;
    }

    public static IEventBusBuilder AddSubscription<TEvent, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(this IEventBusBuilder builder)
        where TEvent : IntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>
    {
        builder.Services.AddKeyedTransient<IIntegrationEventHandler, THandler>(typeof(TEvent));

        builder.Services.Configure<EventBusSubscriptionManager>(opt =>
        {
            opt.EventTypes[typeof(TEvent).FullName!] = typeof(TEvent);
        });

        Console.WriteLine($"{typeof(TEvent).Name} - {typeof(THandler).Name} registered in container.");

        return builder;
    }
}
