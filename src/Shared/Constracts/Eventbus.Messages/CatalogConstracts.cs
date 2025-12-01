using Shared.Core.Events;

namespace Shared.Constracts.Eventbus.Messages;

public record ReserveStockSucceeded(Guid OrderId) : IntegrationEvent;
public record ReserveStockRejected(Guid OrderId) : IntegrationEvent;
public record ReserveStockFailed(Guid OrderId) : IntegrationEvent;