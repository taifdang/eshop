using Application.Catalog.Products.Services;
using Application.Common.Behaviors;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        // MediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        // Behavior
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        // Service
        builder.Services.AddScoped<IImageLookupService, ImageLookupService>();

        return builder;
    }
}
