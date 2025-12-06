namespace Domain.Entities;

public class ProductOption
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public bool AllowImage { get; set; }
    public List<OptionValue> Values { get; set; } = [];
}