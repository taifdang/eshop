using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceDefaults.DataProtection;

public static class Extensions
{
    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
    {
        // Data Protection-keys: cookie auth, session, identity, antiforgery => persist key, encryptor
        services.AddDataProtection().PersistKeysToFileSystem(
            new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = ".AspNetCore"; //example

            options.Events = new CookieAuthenticationEvents
            {

                OnRedirectToLogin = context =>
                {
                    context.Response.Cookies.Delete(options.Cookie.Name); // delete cookie global
                    context.Response.Cookies.Delete("refreshToken");
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
