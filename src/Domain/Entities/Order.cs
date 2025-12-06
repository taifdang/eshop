using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.ValueObject;

namespace Domain.Entities;

public class Order : Aggregate<Guid>
{
    public Guid CustomerId { get; init; }
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; init; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public string? CardBrand { get; set; }
    public string? TransactionId { get; set; }
    public DateTime OrderDate { get; init; } = DateTime.Now;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public static Order Create(Guid orderId, Guid customerId, Address shippingAddress,
        List<OrderItem> items, Money totalAmount)
    {
        var order = new Order 
        { 
            Id = orderId,
            CustomerId = customerId,
            TotalAmount = totalAmount,
            ShippingAddress = shippingAddress,
            Items = items,
            OrderDate = DateTime.Now,
            CreatedAt = DateTime.UtcNow
        };
        order.AddDomainEvent(new OrderCreatedDomainEvent(order.Id, customerId));
        return order;
    }

    public void SetCompletedStatus(Guid orderId)
    {
        Status = OrderStatus.Completed;
        AddDomainEvent(new OrderCompletedDomainEvent(orderId));
    }

    public void SetConfirmedStatus(Guid orderId)
    {
        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderStatusChangedToConfirmedDomainEvent(orderId));
    }

    public void MarkPaymentSuccess(string transactionId)
    {
        LastModified = DateTime.UtcNow;
        Status = OrderStatus.Confirmed;
    }

    public void MarkPaymentFailed()
    { 
        LastModified = DateTime.UtcNow;
        Status = OrderStatus.Failed;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }
}
