using Domain.Common;

namespace Domain.Events;

public record OrderCreatedEvent(Guid OrderId, Guid CustomerId, 
    List<StockReservationItem> Items) : IDomainEvent;

public record StockReservationItem(Guid ProductVariantId, int Quantity);
