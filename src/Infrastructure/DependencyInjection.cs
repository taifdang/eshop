using Application.Basket.EventHandlers;
using Application.Catalog.Products.EventHandlers;
using Application.Common.Interfaces;
using Application.Customer.EventHandlers;
using Application.Order.IntegrationEventHandlers;
using Contracts.IntegrationEvents;
using EventBus;
using EventBus.Abstractions;
using EventBus.RabbitMQ;
using Infrastructure.ExternalServices.Notifications.Email;
using Infrastructure.ExternalServices.Payment;
using Infrastructure.ExternalServices.Payment.Vnpay;
using Infrastructure.ExternalServices.Payment.Stripe;
using Infrastructure.ExternalServices.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.EF.Extensions;
using Outbox.EF.Infrastructure.Data;
using Shared.Constants;
using Shared.Web;
using Persistence;
using Migrator;
using Application.Abstractions;

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

        // Add Persistence Layer
        builder.AddPersistence();

        // External Services
        builder.Services.AddTransient<IEmailService, SmtpEmailSender>();
        builder.Services.AddScoped<IFileService, LocalStorage>();
               

        builder.Services.AddScoped<IPaymentGateway, VnpayPaymentGateway>();
        builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();
        builder.Services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();

        // register Vnpay implementation explicitly
        builder.Services.AddScoped<VnpayPaymentGateway>();

        // bind Stripe options and gateway
        builder.Services.AddScoped<StripePaymentGateway>();

        // Eventbus
        if (builder.Environment.EnvironmentName == "test")
        {
            builder.Services.AddTransient<IEventPublisher, NullEventPublisher>();
        }
        else
        {
            builder.AddRabbitMqEventBus("rabbitmq")
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

    public static WebApplication MapInfrastructure(this WebApplication app)
    {
        //app.UseCustomHealthCheck();

        return app;
    }

    public static async Task<WebApplication> MigrateAndSeedDataAsync(this WebApplication app)
    {
        await app.MigratePersistenceAsync();
        await app.MigrationDbContextAsync<OutboxDbContext>();

        return app;
    }
}
