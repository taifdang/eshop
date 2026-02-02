using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace ServiceDefaults.OpenApi;

//ref: https://github.com/dotnet/eShop/blob/main/src/eShop.ServiceDefaults/OpenApi.Extensions.cs
public static class Extensions
{
    public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<SecuritySchemeDocumentTransformer>();
        });

        return services;
    }
    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        app.MapOpenApi();

        // Swagger UI docs
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "swagger";
            var openApiUrl = "/openapi/v1.json";
            var name = "Open API";

            options.SwaggerEndpoint(openApiUrl, name);
        });

        // Scalar UI docs
        app.MapScalarApiReference(
             redocOptions =>
             {
                 redocOptions.WithOpenApiRoutePattern("/openapi/{documentName}.json");
             });

        return app;
    }
}
