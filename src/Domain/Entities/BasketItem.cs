namespace Domain.Entities;

public class BasketItem
{
    public Guid BasketId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
}
