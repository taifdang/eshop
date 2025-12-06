namespace Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string UrlSlug { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public Guid CategoryId { get; set; } = default!;
    public Category Category { get; set; } = default!;
    public List<ProductImage> Images { get; set; } = [];
    public List<ProductOption> Options { get; set; } = [];
    public List<Variant> Variants { get; set; } = [];
}