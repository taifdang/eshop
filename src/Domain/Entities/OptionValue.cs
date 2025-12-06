using System.Text.Json.Serialization;

namespace Domain.Entities;

public class OptionValue
{
    public Guid Id { get; set; }
    public Guid OptionId { get; set; }
    public string Value { get; set; } = default!;
    public Guid? ImageId { get; set; }
    [JsonIgnore]
    public Image? Image { get; set; }
}