using Domain.Common;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities;

public class Order : Aggregate<Guid>
{
    public Guid CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public static Order Create(Guid orderId, Guid customerId, string shippingAddress, List<OrderItem> items, decimal totalAmount)
    {
        var order = new Order 
        { 
            Id = orderId,
            CustomerId = customerId,
            TotalAmount = totalAmount,
            ShippingAddress = shippingAddress,
            Status = OrderStatus.Pending,
            Items = items,
            OrderDate = DateTime.Now,
            CreatedAt = DateTime.UtcNow
        };

        var orderItems = 
            items.Select(x => new StockReservationItem(x.ProductVariantId, x.Quantity)).ToList();

        order.AddDomainEvent(new OrderCreatedEvent(order.Id, customerId, orderItems));

        return order;
    }
}
