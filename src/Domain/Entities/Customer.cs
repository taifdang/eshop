using Domain.Common;

namespace Domain.Entities;

public class Customer : Aggregate<Guid>
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    //public ICollection<Basket.Entities.Basket> Carts { get; set; }
    //public ICollection<Order.Entities.Order> Orders { get; set; }
}
