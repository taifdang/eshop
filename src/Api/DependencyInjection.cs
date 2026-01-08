using Api.Extensions;
using Api.Services;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;
using ServiceDefaults.OpenApi;

namespace Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddWebServices(this WebApplicationBuilder builder)
    {
        //var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() 
        //    ?? new[] { "http://localhost:3000", "http://localhost:5173" };

        //builder.Services.AddCors(options =>
        //{
        //    options.AddPolicy("AllowFrontend",
        //            policy => policy
        //            .WithOrigins(allowedOrigins)
        //            .AllowAnyMethod()
        //            .AllowAnyHeader()
        //            .AllowCredentials());
        //});

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
            options.Secure = CookieSecurePolicy.SameAsRequest;
        });

        builder.Services.AddJwt();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDefaultOpenApi();

        builder.Services.AddControllers();

        builder.Services.AddHttpContextAccessor();

        // Data Protection-keys: cookie auth, session, identity, antiforgery => persist key, encryptor
        builder.Services.AddDataProtection().PersistKeysToFileSystem(
            new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));

        builder.Services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddTransient<ICookieService, CookieService>();

        //builder.Services.AddHostedService<GracePeriodBackgroundService>();

        return builder;
    }

    public static WebApplication UseWebServices(this WebApplication app)
    {
        // app.UseHttpsRedirection();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseRouting();    

        //app.UseCors("AllowFrontend");
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();
       
        app.MapControllers();
        app.MapDefaultEndpoints();

        app.MapGet("/", x => x.Response.WriteAsync("server listening ..."));

        if (app.Environment.IsEnvironment("Docker") || app.Environment.IsDevelopment())
        {
            app.UseDefaultOpenApi();
        }

        return app;
    }
}
