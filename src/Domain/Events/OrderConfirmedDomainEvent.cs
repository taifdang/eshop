namespace Domain.Events;

public record OrderConfirmedDomainEvent(Guid OrderId) : IDomainEvent;
