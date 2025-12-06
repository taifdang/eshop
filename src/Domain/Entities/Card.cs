namespace Domain.Entities;

public class Card
{
    // here this code not security, but testing enviroment
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CardNumber { get; set; } = default!;
    public string CardHolderName { get; set; } = default!;
    public string ExpirationDate { get; set; } = default!;
    public string Cvv { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string BillingAddress { get; set; } = default!;
    public string BillingCity { get; set; } = default!;  
}