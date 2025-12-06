using System.Text.Json.Serialization;

namespace Domain.Entities;

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid ImageId { get; set; }
    public bool IsMain { get; set; }
    public int SortOrder { get; set; }
    [JsonIgnore]
    public Image Image { get; set; } = default!;
}