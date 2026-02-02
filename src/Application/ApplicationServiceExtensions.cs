using Application.Behaviors;
using Application.Catalog.Products.Services;
using Application.Common.Diagnotics.Commands;
using Application.Common.Diagnotics.Queries;
using Application.Common.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationServiceExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IVariantGenerator, VariantGenerator>();
        builder.Services.AddScoped<IProductVariantService, ProductVariantService>();
        builder.Services.AddScoped<IProductImageResolver, ProductImageResolver>();

        // MediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceExtensions).Assembly);
        });

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(ApplicationServiceExtensions).Assembly);

        // Monitoring
        builder.Services.AddSingleton<CommandHandlerMetrics>();
        builder.Services.AddSingleton<QueryHandlerMetrics>();
        builder.Services.AddSingleton<CommandHandlerActivity>();
        builder.Services.AddSingleton<QueryHandlerActivity>();

        // Behavior
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ActivityBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TxBehavior<,>));

        return builder;
    }
}
