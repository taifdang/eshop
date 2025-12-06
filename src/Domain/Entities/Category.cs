namespace Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string UrlSlug { get; set; } = default!;
}