using Domain.SeedWork;

namespace Domain.Entities;

public class Basket : Entity<Guid>
{
    public Guid CustomerId { get; set; }
    public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();

    public void ClearItems() 
    { 
        Items.Clear();
    }
}
