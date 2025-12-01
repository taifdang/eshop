using Domain.Enums;
using Domain.ValueObject;

namespace Application.Common.Interfaces.Services;

public interface IPaymentProvider
{
    Task<PaymentResult> CreatePaymentAsync(CreatePaymentRequest request);
    Task<bool> HandleCallbackAsync(IDictionary<string, string> queryParams);
  
}
public record PaymentResult(string PaymentUrl, string TransactionId);

public record CreatePaymentRequest
{
    public Guid OrderId { get; init; }
    public Money Amount { get; init; }
    public PaymentProvider Provider { get; init; }
    //public string ReturnUrl { get; init; } = null!;
    //public string IpnUrl { get; init; } = null!;
}

//Task<bool> VerifyReturnUrlAsync(Dictionary<string, string> queryParams);
//Task<bool> VerifyIpnAsync(Dictionary<string, string> queryParams);