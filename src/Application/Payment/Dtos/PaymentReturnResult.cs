namespace Application.Payment.Dtos;

public class PaymentReturnResult
{
    public bool IsValid { get; set; }
    public string? RspCode { get; set; }
    public string? Message { get; set; }
}
