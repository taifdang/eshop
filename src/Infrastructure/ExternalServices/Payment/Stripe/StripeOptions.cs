namespace Infrastructure.ExternalServices.Payment.Stripe;

public class StripeOptions
{
    public string SecretKey { get; set; } = null!; // Stripe secret API key (test key)
    public string WebhookSecret { get; set; } = null!; // webhook signing secret (test)
    public string ReturnUrl { get; set; } = null!; // e.g. https://example.com/payment
    public string Currency { get; set; } = "vnd";
}
