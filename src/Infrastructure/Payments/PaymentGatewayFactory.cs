using Application.Common.Interfaces;
using Domain.Enums;
using Infrastructure.Payments.Gateways;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Payments;

public class PaymentGatewayFactory 
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentGatewayFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    //public IPaymentProvider GetPaymentProvider(PaymentProvider provider)
    //{
    //    return provider switch
    //    {
    //        PaymentProvider.Vnpay => _serviceProvider.GetRequiredService<VnPayGateway>(),
    //        PaymentProvider.Paypal => _serviceProvider.GetRequiredService<PaypalGateway>(),
    //        _ => throw new NotSupportedException("Not supports this payment prodvider")
    //    };
    //}
}
