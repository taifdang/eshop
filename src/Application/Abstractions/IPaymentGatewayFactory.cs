using Domain.Enums;

namespace Application.Abstractions;

public interface IPaymentGatewayFactory
{
    IPaymentGateway Resolve(PaymentProvider provider);
}
