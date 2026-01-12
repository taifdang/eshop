using Api.Extensions;
using Api.Services;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using ServiceDefaults.OpenApi;

namespace Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddWebDependencies(this WebApplicationBuilder builder)
    {
#if (crossDomainUsingCors)
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins");
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                    policy => policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });
#endif

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
        builder.Services.AddCustomDataProtection();

        builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
        builder.Services.AddScoped<ICookieService, CookieService>();
        builder.Services.AddScoped<ICurrentIPAddressProvider, CurrentIPAddressProvider>();

        //builder.Services.AddHostedService<GracePeriodBackgroundService>();

        return builder;
    }

    public static WebApplication UseWebDependencies(this WebApplication app)
    {
        //app.UseHttpsRedirection();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            // Loopback by default, this clears that out so we can accept from any proxy
            KnownNetworks = { }, 
            KnownProxies = { },
        });

        app.UseRouting();

#if (crossDomainUsingCors)
        app.UseCors("AllowFrontend");
#endif
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
