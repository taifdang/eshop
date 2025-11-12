using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Constants;

namespace Infrastructure;
//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio
public static class Dependencies
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<ApplicationDbContext>(p => p.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));

        services.AddDbContext<AppIdentityDbContext>(p => p.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection));
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        if (appSettings.FileStorageSettings.LocalStorage)
        {
            services.AddSingleton<IFileService, LocalStorageService>();
        }

        //services.AddAutoMapper(typeof(ApplicationRoot).Assembly);
        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, LocalStorageService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IUserManagerService, UserManagerService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
