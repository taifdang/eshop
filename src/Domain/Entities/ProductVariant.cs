using Domain.Enums;

namespace Domain.Entities;

public class ProductVariant
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Title { get; set; } // Product non-option specific title
    public decimal Price { get; set; }
    public int Quantity { get; set; } // Inventory 
    public decimal Percent { get; set; } // Discount Percent
    public string? Sku { get; set; }
    public IntentoryStatus Status { get; set; } // Inventory
    public Product Product { get; set; } 
    public ICollection<VariantOptionValue> VariantOptionValues { get; set; } = new List<VariantOptionValue>();
}
