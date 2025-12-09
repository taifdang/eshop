namespace Domain.Entities;

public class Variant
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Sku { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }   
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public Product Product { get; set; }
    public List<VariantOption> VariantOptions { get; set; } = [];

    public void ReserveStock(int quantity)
    {
        if (Quantity < quantity)
            throw new ArgumentException("Not enough quantity", nameof(quantity));

        Quantity -= quantity;
    }
}
