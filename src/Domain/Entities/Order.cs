using Domain.Enums;
using Domain.Events;
using Domain.SeedWork;
using Domain.ValueObject;

namespace Domain.Entities;

public class Order : Aggregate<Guid>
{
    public long OrderNumber { get; set; } // for payment
    public Guid CustomerId { get; init; }
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public string? CardBrand { get; set; }
    public string? TransactionId { get; set; }
    public DateTime OrderDate { get; init; } = DateTime.UtcNow;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public static Order Create(Guid orderId, long OrderNumber, Guid customerId, Address shippingAddress,
        List<OrderItem> items, Money totalAmount)
    {
        var order = new Order 
        { 
            Id = orderId,
            OrderNumber = OrderNumber,
            CustomerId = customerId,
            TotalAmount = totalAmount,
            ShippingAddress = shippingAddress,
            Items = items,
            OrderDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };
        order.AddDomainEvent(new OrderCreatedDomainEvent(order.Id, customerId));
        return order;
    }

    public void SetProcessingStatus()
    {
        Status = OrderStatus.Processing;
    }

    public void SetCompletedStatus()
    {
        Status = OrderStatus.Completed;
        AddDomainEvent(new OrderCompletedDomainEvent(Id));
    }

    public void SetConfirmedStatus()
    {
        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmedDomainEvent(Id));
    }

    public void SetCancelledStatus()
    {
        if(Status == OrderStatus.Pending)
        {
            throw new Exception("Can't cancel while order processing");
        }
        // description / reason
        Status = OrderStatus.Cancelled;
        AddDomainEvent(new OrderCancelledDomainEvent(Id));
    }

    public void SetRejectedStatusWhenStockRejected()
    {
        if (Status == OrderStatus.Pending)
        {
            throw new Exception("Can't cancel while order processing");
        }
        // description / reason
        Status = OrderStatus.Rejected;
        AddDomainEvent(new OrderRejectedDomainEvent(Id));
    }

    public void SetRejectedStatusWhenPaymentRejected()
    {
        if (Status == OrderStatus.Pending)
        {
            throw new Exception("Can't cancel while order processing");
        }
        // description / reason
        Status = OrderStatus.Rejected;
        AddDomainEvent(new OrderRejectedDomainEvent(Id));
    }
}
