using Shared.EFCore;
using Shared.Jwt;
using Shared.OpenApi;
using Shared.Web;
using System.Text.Json.Serialization;

namespace Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();
        builder.Services.AddAspnetOpenApi();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddJwt();

        builder.Services.AddTransient<ICurrentUserProdvider, CurrentUserProvider>();
        builder.Services.AddScoped<ICookieService, CookieService>();

        builder.Services.AddControllers()
            .AddJsonOptions(opt =>
            {
                // Handle reference loops
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                opt.JsonSerializerOptions.WriteIndented = true;

            });

        return builder;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {  
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
