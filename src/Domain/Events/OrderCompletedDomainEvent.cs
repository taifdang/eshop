namespace Domain.Events;

public record OrderCompletedDomainEvent(Guid OrderId) : IDomainEvent;