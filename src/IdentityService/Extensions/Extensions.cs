using Duende.IdentityServer;
using EventBus.Abstractions;
using EventBus.RabbitMQ;
using IdentityService.ConfigurationOptions.ExternalLogin;
using IdentityService.Configurations;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Entity;
using IdentityService.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Migrator;

namespace IdentityService.Extensions;

public static class Extensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<IdentityContext>("shopdb");

        // Configure Identity
        builder.Services.AddIdentity<User, Role>(config =>
        {
            config.Password.RequiredLength = 6;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

        // Configure IdentityServer
        var identityServerBuilder = builder.Services
        .AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
            options.EmitStaticAudienceClaim = true;
        })
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryApiResources(Config.ApiResources)
        .AddInMemoryClients(Config.Clients)
        .AddAspNetIdentity<User>();

        if (builder.Environment.IsDevelopment()) identityServerBuilder.AddDeveloperSigningCredential();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
        });

        // Configure AppSettings
        var externalLogin = builder.Configuration.GetSection("ExternalLogin").Get<ExternalLoginOptions>();

        // Add external authentication providers
        var authenticationBuilder = builder.Services.AddAuthentication();

        if (externalLogin?.Google?.IsEnabled == true)
        {
            authenticationBuilder.AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = externalLogin.Google.ClientId;
                options.ClientSecret = externalLogin.Google.ClientSecret;
            });
        }

        if (externalLogin?.Facebook?.IsEnabled == true)
        {
            authenticationBuilder.AddFacebook("Facebook", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.AppId = externalLogin.Facebook.AppId;
                options.AppSecret = externalLogin.Facebook.AppSecret;
            });
        }

        builder.Services.AddScoped<IDataSeeder<IdentityContext>, DataSeeder>();

        if (builder.Environment.EnvironmentName == "test")
        {
            builder.Services.AddTransient<IEventPublisher, NullEventPublisher>();
        }
        {
            builder.AddRabbitMqEventBus("rabbitmq");
        }

        return builder;
    }
}