using Domain.ValueObject;

namespace Application.Common.Interfaces;

public interface IPaymentProvider
{
    Task<PaymentResult> CreatePaymentAsync(CreatePaymentRequest request);
    Task<bool> HandleCallbackAsync(IDictionary<string, string> queryParams);
  
}
public record PaymentResult(string PaymentUrl, string TransactionId);

public record CreatePaymentRequest
{
    public long OrderNumber { get; init; }
    public decimal Amount { get; init; }
    //public PaymentProvider Provider { get; init; }
    //public string ReturnUrl { get; init; } = null!;
    //public string IpnUrl { get; init; } = null!;
}

//Task<bool> VerifyReturnUrlAsync(Dictionary<string, string> queryParams);
//Task<bool> VerifyIpnAsync(Dictionary<string, string> queryParams);