using Application.Basket.EventHandlers;
using Application.Catalog.Products.EventHandlers;
using Application.Common.Interfaces;
using Application.Customer.EventHandlers;
using Application.Order.IntegrationEventHandlers;
using Contracts.IntegrationEvents;
using EventBus;
using EventBus.Abstractions;
using EventBus.InMemory;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.Seed;
using Infrastructure.ExternalServices;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Data.Seed;
using Infrastructure.Payments.Gateways;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.EF.Extensions;
using Outbox.EF.Infrastructure.Data;
using Shared.Constants;
using Shared.EFCore;
using Shared.Web;
using System;

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
#if (sqlserver)
            //options.useSqlServer(appSettings.ConnectionStrings.DefaultConnection);
#endif
            options.UseNpgsql(builder.Configuration.GetConnectionString("shopdb"));
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });

        builder.AddNpgsqlDbContext<AppIdentityDbContext>("shopdb");

        builder.AddCustomIdentity();

        // Jwt
        builder.Services.AddScoped<ITokenService, TokenService>();

        // Repositores    
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        // Custom Servicess      
        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddScoped<IFileService, LocalStorageService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IPaymentProvider, PaypalGateway>();
        builder.Services.AddScoped<IPaymentProvider, VnPayGateway>();

        // Seeders    
        builder.Services.AddScoped<ISeedManager, SeedManager>();

        builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();
        builder.Services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        // eventbus
        if (builder.Environment.EnvironmentName == "test")
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
           .AddSubscription<ReserveStockSucceededIntegrationEvent, ReserveStockSucceededIntegrationEventHandler>()
           .AddSubscription<UserCreatedIntegrationEvent, UserCreatedIntegrationEventHandler>();
        }
        builder.AddTransactionalOutbox();

        return builder;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {

        app.UseMigration<ApplicationDbContext>();
        app.UseMigration<AppIdentityDbContext>();
        app.UseMigration<OutboxDbContext>();

        return app;
    }
}
