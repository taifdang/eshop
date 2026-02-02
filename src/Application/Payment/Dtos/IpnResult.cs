namespace Application.Payment.Dtos;

public class IpnResult
{
    public string RspCode { get; set; } = null!;
    public string Message { get; set; } = null!;
}
