using Domain.Enums;

namespace Domain.Entities;

public class ProductVariant
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Percent { get; set; }
    public string? Sku { get; set; }
    public IntentoryStatus Status { get; set; }
    public Product Product { get; set; } 
    public ICollection<VariantOptionValue> VariantOptionValues { get; set; } = new List<VariantOptionValue>();

    public void DeductStock(int quantity)
    {
        if (Quantity < quantity)
            throw new ArgumentException("Not enough quantity", nameof(quantity));

        Quantity -= quantity;
    }
}
