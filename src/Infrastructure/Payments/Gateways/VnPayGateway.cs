using Application.Common.Interfaces;

namespace Infrastructure.Payments.Gateways;

public class VnPayGateway : IPaymentProvider
{
    public Task<PaymentResult> CreatePaymentAsync(CreatePaymentRequest request)
    {
        var url = $"https://vnpay.vn/pay?order={request.OrderNumber}";
        return Task.FromResult(new PaymentResult(url, DateTime.UtcNow.Ticks.ToString()));
    }

    public Task<bool> HandleCallbackAsync(IDictionary<string, string> queryParams)
    {
        // validate

        return Task.FromResult(true);
    }
}
