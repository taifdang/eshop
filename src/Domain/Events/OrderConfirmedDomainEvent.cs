namespace Domain.Events;

public record OrderConfirmedDomainEvent(Guid OrderId, Guid CustomerId, decimal TotalAmount) : IDomainEvent;
