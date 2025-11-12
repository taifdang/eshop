using Infrastructure;
using Shared.Constants;

namespace Api.Extensions;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, AppSettings appsettings)
    {
        builder.Services.AddInfrastructureService(appsettings);
        //builder.Services.AddApplicationService(appsettings);
        builder.Services.AddWebAPIService(appsettings);

        //builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())); 

        return builder;
    }
    public static WebApplication ConfigurePipelineAsync(this WebApplication app, AppSettings appsettings)
    {
        using var loggerFactory = LoggerFactory.Create(builder => { });
        using var scope = app.Services.CreateScope();
  
        app.UseHttpsRedirection();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.MapControllers();
       
        return app;
    }
}
