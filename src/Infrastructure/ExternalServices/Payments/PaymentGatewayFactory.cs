using Application.Common.Interfaces;
using Domain.Enums;
using Infrastructure.ExternalServices.Payments.Stripe;
using Infrastructure.ExternalServices.Payments.Vnpay;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExternalServices.Payments;

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
            //PaymentProvider.Paypal => _serviceProvider.GetRequiredService<PaypalPaymentGateway>(), //not implemention
            _ => throw new NotSupportedException()
        };
    }
}