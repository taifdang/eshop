using Infrastructure.ExternalServices.Storage.Local;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExternalServices.Storage;

public static class StorageCollectionExtensions
{
    public static IServiceCollection AddLocalStorageManager(this IServiceCollection services, LocalOptions options)
    {
        services.AddSingleton<IFileStorageManager>(new LocalFileStorageManager(options));
        return services;
    }

    public static IServiceCollection AddStorageManager(this IServiceCollection services, StorageOptions options)
    {
        services.AddLocalStorageManager(options.Local);
        return services;
    }
}
