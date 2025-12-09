using Application.Basket.EventHandlers;
using Application.Catalog.Products.EventHandlers;
using Application.Common.Interfaces;
using Application.Order.IntegrationEventHandlers;
using Contracts.IntegrationEvents;
using EventBus;
using EventBus.Abstractions;
using EventBus.InMemory;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Data.Repositories;
using Infrastructure.ExternalServices;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Extensions;
using Infrastructure.Identity.Services;
using Infrastructure.Payments.Gateways;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Outbox.EF.Extensions;
using Shared.Constants;
using Shared.EFCore;
using Shared.Web;

namespace Infrastructure;
//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio
public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        // Get Configuration
        var appSettings = builder.Configuration.GetOptions<AppSettings>();
        builder.Services.AddSingleton(appSettings);

        // Interceptors
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventIntercopter>();

        // DbContext
        //builder.AddCustomDbContext<ApplicationDbContext>();
        // Inteceptors
        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(appSettings.ConnectionStrings.DefaultConnection);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        // Identity
        builder.AddCustomDbContext<AppIdentityDbContext>();
        builder.AddCustomIdentity();

        // Jwt
        builder.Services.AddScoped<ITokenService, TokenService>();

        // Repositores    
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        // Custom Servicess      
        builder.Services.AddTransient<IEmailService, EmailService>();
        //if (appSettings.FileStorageSettings.LocalStorage)
        //{
        //    builder.Services.AddSingleton<IFileService, LocalStorageService>();
        //}
        builder.Services.AddScoped<IFileService, LocalStorageService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IUserManagerService, UserManagerService>();

        builder.Services.AddScoped<IPaymentProvider, PaypalGateway>();
        builder.Services.AddScoped<IPaymentProvider, VnPayGateway>();

        // Seeders    
        //builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();
        //builder.Services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        // eventbus
        if(builder.Environment.EnvironmentName == "test")
        {
            builder.Services.AddTransient<IEventPublisher, NullEventPublisher>();
        }
        else
        {
            builder.AddInMemoryEventBus()
           .AddSubscription<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>()
           .AddSubscription<StockReservationRequestedIntegrationEvent, StockReservationRequestedIntegrationEventHandler>()
           .AddSubscription<GracePeriodConfirmedIntegrationEvent, GracePeriodConfirmedIntegrationEventHandler>()
           .AddSubscription<PaymentSucceededIntegrationEvent, PaymentSucceededIntegrationEventHandler>()
           .AddSubscription<PaymentRejectedIntegrationEvent, PaymentRejectedIntegrationEventHandler>()
           .AddSubscription<ReserveStockRejectedIntegrationEvent, ReserveStockRejectedIntegrationEventHandler>()
           .AddSubscription<ReserveStockSucceededIntegrationEvent, ReserveStockSucceededIntegrationEventHandler>();
        }
        builder.AddTransactionalOutbox();

        return builder;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.UseMigration<AppIdentityDbContext>();

        return app;
    }
}
