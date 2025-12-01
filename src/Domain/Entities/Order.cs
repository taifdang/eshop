using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.ValueObject;

namespace Domain.Entities;

public class Order : Aggregate<Guid>
{
    public Guid CustomerId { get; init; }
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; init; }
    public Address ShippingAddress { get; private set; }
    public Payment? Payment { get; private set; }
    public DateTime OrderDate { get; init; } = DateTime.Now;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public static Order Create(Guid orderId, Guid customerId, Address shippingAddress,
        List<OrderItem> items, Money totalAmount, Payment payment)
    {
        var order = new Order 
        { 
            Id = orderId,
            CustomerId = customerId,
            TotalAmount = totalAmount,
            ShippingAddress = shippingAddress,
            Status = payment.Method == PaymentMethod.Cod 
                ? OrderStatus.Confirmed 
                : OrderStatus.Pending,
            Items = items,
            OrderDate = DateTime.Now,
            CreatedAt = DateTime.UtcNow
        };

        //var orderItems = 
        //    items.Select(x => new OrderItemDto(x.ProductVariantId, x.Quantity)).ToList();

        order.AddDomainEvent(new OrderCreatedDomainEvent(order.Id, customerId));

        //if (payment.Method == PaymentMethod.Cod)
        //{
        //    order.AddDomainEvent(new BasketShouldBeClearedEvent(customerId));
        //}

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


    public void AddPayment(PaymentProvider provider, string paymentUrl, string transactionId)
    {
        if (Payment.Method != PaymentMethod.Online)
            throw new Exception("Only online payment can have URL");
        Payment = Payment.WithPayment(provider, paymentUrl, transactionId);
    }

    public void MarkPaymentSuccess(string transactionId)
    {
        if (Payment == null)
            throw new InvalidOperationException("Payment not exist");
        Payment = Payment.MarkAsPaid(transactionId);
        Status = OrderStatus.Confirmed;
    }

    public void MarkPaymentFailed()
    {
        if (Payment == null) 
            throw new InvalidOperationException("Payment not exist");
        Payment = Payment.MarkAsFailed();
        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
    }
}
