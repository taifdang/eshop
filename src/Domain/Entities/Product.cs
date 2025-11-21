using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Product : Entity<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ProductStatus Status { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
    public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}
