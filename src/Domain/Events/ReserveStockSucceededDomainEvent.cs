namespace Domain.Events;

public record ReserveStockSucceededDomainEvent(Guid OrderId) : IDomainEvent;