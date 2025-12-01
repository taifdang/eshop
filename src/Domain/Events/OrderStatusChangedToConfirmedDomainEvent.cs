namespace Domain.Events;

public record OrderStatusChangedToConfirmedDomainEvent(Guid OrderId) : IDomainEvent;
