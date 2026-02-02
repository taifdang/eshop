using Domain.Enums;

namespace Application.Order.Dtos;

public class CreateOrderResult
{
    public Guid OrderId { get; set; }
    public long OrderNumber { get; set; }
    public decimal Amount { get; set; }
    public Guid CustomerId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentProvider? PaymentProvider { get; set; }
}
