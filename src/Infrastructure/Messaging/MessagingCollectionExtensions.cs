using Infrastructure.Messaging.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddInmemorySender<T>(this IServiceCollection services, InMemoryOptions options)
    {
        // configure inmemory sender here
        return services;
    }
    public static IServiceCollection AddMessageBusSender<T>(this IServiceCollection services, MessagingOptions options)
    {
        if (options.UseInmemory())
        {
            services.AddInmemorySender<T>(options.InMemory);
        }
        // implement other providers here

        return services;
    }
}
