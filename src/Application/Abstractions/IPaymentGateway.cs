namespace Application.Abstractions;

public interface IPaymentGateway
{
    Task<CreatePaymentUrlResult> CreatePaymentUrl(CreatePaymentUrlRequest request);

    VerifyReturnUrlResult VerifyReturnUrl(IDictionary<string, string> parameters);

    VerifyIpnResult VerifyIpnCallback(IDictionary<string, string> parameters);
}

public class CreatePaymentUrlRequest
{
    public long OrderNumber { get; init; }
    public decimal Amount { get; init; }
    public DateTime OrderDate { get; init; }
}

public class CreatePaymentUrlResult
{
    public bool Status { get; set; }
    public string? Data { get; set; }
    public string? Error { get; set; }
}

public class VerifyIpnResult
{
    public bool CheckSignature { get; set; }
    public bool IsSuccess { get; set; }
    public bool IsNullEvent { get; set; }
    public string ResCode { get; set; } = null!;
    // destructure properties to Ipn 
    public long OrderNumber { get; set; }
    public string TransactionId { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? CardBrand { get; set; } = "N/A";
}
public class VerifyReturnUrlResult
{
    public bool CheckSignature { get; set; }
    public bool IsSuccess { get; set; }
    public string ResCode { get; set; } = null!;
}

