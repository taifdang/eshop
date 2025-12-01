using Application.Common.Interfaces.Services;

namespace Infrastructure.Payments.Gateways;

public class VnPayGateway : IPaymentProvider
{
    public Task<PaymentResult> CreatePaymentAsync(CreatePaymentRequest request)
    {
        var url = $"https://vnpay.vn/pay?order={request.OrderId}";
        return Task.FromResult(new PaymentResult(url, DateTime.UtcNow.Ticks.ToString()));
    }

    public Task<bool> HandleCallbackAsync(IDictionary<string, string> queryParams)
    {
        // validate

        return Task.FromResult(true);
    }
}
