namespace Domain.Entities;

public class VariantOption
{
    public Guid VariantId { get; set; }
    public Guid OptionValueId { get; set; }
    public OptionValue OptionValue { get; set; }
}
