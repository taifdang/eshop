using Domain.Common;

namespace Domain.Entities;

public class Category : Aggregate<Guid>
{
    public string Title { get; set; }
    public string? Label { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
