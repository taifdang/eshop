using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
        // Map OpenAPI documents (produces /openapi/v1.json)
        app.MapOpenApi();

        // Serve Swagger UI under /openapi/swagger
        app.UseSwaggerUI(options =>
        {
            var openApiUrl = "/openapi/v1.json";
            var name = "Open API";
            options.SwaggerEndpoint(openApiUrl, name);
            options.RoutePrefix = "openapi/swagger";
        });

        return app;
    }
}
