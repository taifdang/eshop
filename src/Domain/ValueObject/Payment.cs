using Domain.Enums;

namespace Domain.ValueObject;

public record Payment
{
    public string? TransactionId { get; init; }
    public PaymentProvider Provider { get; init; }
    public Money Amount { get; init; }
    public PaymentMethod Method { get; init; }
    public PaymentStatus Status { get; init; }
    public string? PaymentUrl { get; init; }
    public DateTime? PaidAt { get; init; }

    private Payment(PaymentProvider provider, Money amount, PaymentMethod method, string? paymentUrl = null) 
    {
        Provider = provider;
        Method = method;
        PaymentUrl = paymentUrl;
        Amount = amount;
        Status = PaymentStatus.Pending;
    }

    public static Payment Of(PaymentProvider provider, Money amount, PaymentMethod method, string? paymentUrl = null)
    {
        if (amount.Amount < 0) 
            throw new ArgumentOutOfRangeException("Amount can not negative.");
        if (method == PaymentMethod.None)
            throw new ArgumentException("Method must be specified.", nameof(method));

        return new Payment(provider, amount, method, paymentUrl);
    }

    public static Payment CreateCod(Money amount)
    {
        return Of(PaymentProvider.None, amount, PaymentMethod.Cod);
    }

    public static Payment CreateOnline(PaymentProvider provider, Money amount, string paymentUrl)
    {
        if (string.IsNullOrWhiteSpace(paymentUrl))
            throw new ArgumentException("PaymentUrl is required for online payment.", nameof(paymentUrl));

        return Of(provider, amount, PaymentMethod.Online, paymentUrl);
    }

    public Payment WithPayment(PaymentProvider provider, string paymentUrl, string transactionId)
    {
        return this with
        {
            Provider = provider,
            PaymentUrl = paymentUrl,
            TransactionId = transactionId
        };
    }

    public Payment MarkAsPaid(string transactionId)
    {
        if (Status is PaymentStatus.Success or PaymentStatus.Failed)
            throw new InvalidOperationException("Payment process is end. Can not set status");

        return this with
        {
            TransactionId = transactionId,
            Status = PaymentStatus.Success,
            PaidAt = DateTime.UtcNow
        };
    }

    public Payment MarkAsFailed(string? reason = null)
    {
        if (Status is PaymentStatus.Success)
            throw new InvalidOperationException("Payment was success. Can not set status");

        return this with
        {
            Status = PaymentStatus.Failed,
        };
    }

    public Payment MarkAsExpired()
    {
        if (Status is PaymentStatus.Pending)
            throw new InvalidOperationException("Only Payment with status pending can be change set.");

        return this with
        {
            Status = PaymentStatus.Expired,
            PaidAt = DateTime.UtcNow
        };
    }
}

