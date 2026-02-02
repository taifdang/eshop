using Application.Abstractions;
using Domain.Enums;
using Infrastructure.ExternalServices.Payment.Stripe;
using Infrastructure.ExternalServices.Payment.Vnpay;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExternalServices.Payment;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentGatewayFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IPaymentGateway Resolve(PaymentProvider provider)
    {
        return provider switch
        {
            PaymentProvider.Vnpay => _serviceProvider.GetRequiredService<VnpayPaymentGateway>(),
            PaymentProvider.Stripe => _serviceProvider.GetRequiredService<StripePaymentGateway>(),
            _ => throw new NotSupportedException()
        };
    }
}