namespace Domain.Events;

public record ReserveStockRejectedDomainEvent(Guid OrderId, List<StockItems> Items, string? Reason = null) : IDomainEvent;

public record StockItems(Guid variantId, int request, int quantity);