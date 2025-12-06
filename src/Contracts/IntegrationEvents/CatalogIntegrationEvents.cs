using EventBus.Events;

namespace Contracts.IntegrationEvents;

public class ReserveStockSucceededIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class ReserveStockRejectedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class ReserveStockFailedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}
