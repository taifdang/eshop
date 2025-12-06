using Shared.Jwt;
using Shared.OpenApi;
using Shared.Web;

namespace Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
    {
        builder.Services.AddJwt();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAspnetOpenApi();

        builder.Services.AddControllers();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<ICurrentUserProdvider, CurrentUserProvider>();
        builder.Services.AddScoped<ICookieService, CookieService>();

        
            //.AddJsonOptions(opt =>
            //{
            //    // HandleAsync reference loops
            //    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //    opt.JsonSerializerOptions.WriteIndented = true;

            //});

        return builder;
    }

    public static WebApplication UseServiceDefaults(this WebApplication app)
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
