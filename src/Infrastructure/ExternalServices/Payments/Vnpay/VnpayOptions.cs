namespace Infrastructure.ExternalServices.Payments.Vnpay;

public class VnpayOptions
{
    public string Version { get; set; } = "2.1.0";
    public string Command { get; set; } = "pay";
    public string TmnCode { get; set; } = null!;
    public string HashSecret { get; set; } = null!;
    public string CurrCode { get; set; } = "VND";
    public string Locale { get; set; } = "vi";
    public string OrderType { get; set; } = "other";
    public string ReturnUrl { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
}
