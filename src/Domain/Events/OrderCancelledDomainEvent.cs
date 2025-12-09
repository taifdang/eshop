namespace Domain.Events;

public record OrderCancelledDomainEvent(Guid OrderId) : IDomainEvent;

