using EventBus.Events;

namespace Contracts.IntegrationEvents;

public class OrderCreatedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
}

public class OrderCancelledIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class OrderCompletedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class GracePeriodConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class OrderStockRejectedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class OrderStockConfirmedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class OrderPaymentFailedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class OrderPaymentSucceededIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
}

public class StockReservationRequestedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; }
    public IReadOnlyList<OrderStockItem> Items { get; set; }

    public class OrderStockItem
    {
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
    };
}
