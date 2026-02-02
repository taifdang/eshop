using Bff.ConfigurationOptions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using System.Net.Http.Headers;
using Yarp.ReverseProxy.Transforms;

// ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-oidc-web-authentication?view=aspnetcore-9.0

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

builder.AddServiceDefaults();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(transformBuilderContext =>
    {
        transformBuilderContext.AddRequestTransform(async transformContext =>
        {
            var token = await transformContext.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (!string.IsNullOrEmpty(token))
            {
                Console.WriteLine(token);
                transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        });
    });

builder.Services.AddControllers();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "__Bff-Session";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    options.Authority = appSettings.OpenIdConnect?.Authority;
    options.ClientId = appSettings.OpenIdConnect?.ClientId;
    options.ClientSecret = appSettings.OpenIdConnect?.ClientSecret;

    options.ResponseType = "code";
    options.UsePkce = true;

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.RequireHttpsMetadata = false;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("Read");
    options.Scope.Add("Write");

    options.MapInboundClaims = false;

    /* NEW v2: Configure OpenIdConnect events for proper logout handling */
    options.Events = new OpenIdConnectEvents
    {
        OnRedirectToIdentityProviderForSignOut = context =>
        {
            // Ensure the post logout redirect URI points to SPA
            if (string.IsNullOrEmpty(context.ProtocolMessage.PostLogoutRedirectUri))
            {
                context.ProtocolMessage.PostLogoutRedirectUri = "http://localhost:3000/";
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.MapControllers();

app.MapDefaultEndpoints();

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value?.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ?? false)
    {
        try
        {
            var antiForgeryService = context.RequestServices.GetRequiredService<IAntiforgery>();
            await antiForgeryService.ValidateRequestAsync(context);
        }
        catch (AntiforgeryValidationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
    }
    await next(context);
});

app.MapReverseProxy();

app.Run();


