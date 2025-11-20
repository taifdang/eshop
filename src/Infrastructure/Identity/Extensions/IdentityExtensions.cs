using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Extensions;

public static class IdentityExtensions
{
    public static WebApplicationBuilder AddCustomIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
        {
            config.Password.RequiredLength = 6;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
           .AddEntityFrameworkStores<AppIdentityDbContext>()
           .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        });

        return builder;
    }
}
