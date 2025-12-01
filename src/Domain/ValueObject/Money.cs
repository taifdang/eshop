namespace Domain.ValueObject;

public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Of(decimal amount, string currency)
    {
        if (amount < 0) 
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency)) 
            throw new ArgumentException("Currency is required.", nameof(currency));
        
        return new Money(amount, currency.ToUpperInvariant());
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency) 
            throw new InvalidOperationException("Incalculable.");
        return Of(left.Amount + right.Amount, left.Currency);
    }
    public static Money InitValue()
    {
        return Of(0, "VND");
    }
    public static Money Vnd(decimal amount)
    {
        return Of(amount, "VND");
    }
    public static Money Usd(decimal amount)
    {
        return Of(amount, "USD");
    }
}
