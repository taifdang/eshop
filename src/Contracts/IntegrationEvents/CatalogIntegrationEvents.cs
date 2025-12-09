using EventBus.Events;

namespace Contracts.IntegrationEvents;

public class ReserveStockSucceededIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class ReserveStockRejectedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public List<InvalidStockItem>? Items { get; set; }
}

public class InvalidStockItem
{
    public Guid VariantId { get; set; } 
    public int Requested { get; set; }
    public int Available { get; set; }
}