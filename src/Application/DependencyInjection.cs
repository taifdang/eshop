using Application.Catalog.Products.Services;
using Application.Common.Behaviors;
using EventBus.InMemory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TransactionalOutbox.Extensions;

namespace Application;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        // Automapper
        builder.Services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        // Behavior
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        // Service
        builder.Services.AddScoped<IImageLookupService, ImageLookupService>();

        builder.Services.AddInMemoryEventBus();
        // ?
        builder.AddTransactionalOutbox();

        return builder;
    }
}
