using Shared.Core.Events;
namespace Shared.Constracts.Eventbus.Messages;

public record OrderCreated(Guid OrderId, Guid CustomerId) : IntegrationEvent;
public record OrderCancelled(Guid OrderId) : IntegrationEvent;
public record OrderCompleted(Guid OrderId): IntegrationEvent;
public record GracePeriodConfirmed(Guid OrderId) : IntegrationEvent;
public record OrderStockRejected(Guid OrderId) : IntegrationEvent;
public record OrderStockConfirmed(Guid OrderId) : IntegrationEvent;
public record OrderPaymentFailed(Guid OrderId) : IntegrationEvent;
public record OrderPaymentSucceeded(Guid OrderId) : IntegrationEvent;
public record StockReservationRequested(Guid OrderId, IReadOnlyList<OrderStockItem> Items) : IntegrationEvent;
public record OrderStockItem(Guid VariantId, int Quantity);

