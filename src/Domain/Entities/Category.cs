namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Label { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
