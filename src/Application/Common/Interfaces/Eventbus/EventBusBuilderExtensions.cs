using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Interfaces.Eventbus;

public static class EventBusBuilderExtensions
{
    public static IEventBusBuilder AddSubscription<TEvent, TEventHandler>(this IEventBusBuilder builder)
        where TEvent : class
        where TEventHandler : class, IIntegrationEventHandler<TEvent>
    {
        builder.Services.AddKeyedTransient(typeof(TEvent), typeof(TEventHandler));

        builder.Services.Configure<EventBusSubscription>(opt =>
        {
            opt.EventTypes[typeof(TEvent).Name] = typeof(TEventHandler);
        });

        return builder;
    }
}
