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
using Infrastructure.ExternalServices.Notifications.Email;
using Infrastructure.ExternalServices.Payments;
using Infrastructure.ExternalServices.Payments.Vnpay;
using Infrastructure.ExternalServices.Payments.Stripe;
using Infrastructure.ExternalServices.Storage;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Data.Seed;
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

namespace Infrastructure;
//ref: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio
public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        // Get Configuration
        var appSettings = builder.Configuration.GetOptions<AppSettings>();
        builder.Services.Configure<VnpayOptions>(builder.Configuration.GetSection("VnpayConf"));
        builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection("StripeConf"));

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

        builder.AddNpgsqlDbContext<AppIdentityDbContext>("shopdb",
            configureDbContextOptions: x => x.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

        builder.AddCustomIdentity();

        // Jwt
        builder.Services.AddScoped<ITokenService, TokenService>();

        // Repositores    
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<IUnitOfWork, ApplicationDbContext>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
  
        // External Services
        builder.Services.AddTransient<IEmailService, SmtpEmailSender>();
        builder.Services.AddScoped<IFileService, LocalStorage>();
               
        // default IPaymentGateway registration points to Vnpay (legacy). Keep explicit registration for both gateways.
        builder.Services.AddScoped<IPaymentGateway, VnpayPaymentGateway>();
        builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();
        builder.Services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();

        // register Vnpay implementation explicitly
        builder.Services.AddScoped<VnpayPaymentGateway>();

        // bind Stripe options and gateway
        builder.Services.AddScoped<StripePaymentGateway>();

        // Identity
        builder.Services.AddScoped<IIdentityService, IdentityService>();

        // Seeders    
        builder.Services.AddScoped<ISeedManager, SeedManager>();

        builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();
        builder.Services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        // Eventbus
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
