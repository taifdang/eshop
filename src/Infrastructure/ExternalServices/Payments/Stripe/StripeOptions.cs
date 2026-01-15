namespace Infrastructure.ExternalServices.Payments.Stripe;

public class StripeOptions
{
    public string SecretKey { get; set; } = null!; // Stripe secret API key (test key)
    public string WebhookSecret { get; set; } = null!; // webhook signing secret (test)
    public string ReturnUrl { get; set; } = null!; // e.g. https://example.com/payment
    public string SuccessUrl { get; set; } = null!; // e.g. https://example.com/payment/success?session_id={CHECKOUT_SESSION_ID}
    public string CancelUrl { get; set; } = null!; // e.g. https://example.com/payment/cancel
    public string Currency { get; set; } = "vnd";
}
