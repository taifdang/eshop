using EventBus.Events;

namespace Contracts.IntegrationEvents;

public class PaymentSucceededIntegrationEvent : IntegrationEvent
{
    public long OrderNumber { get; set; }
    public string? CardBrand { get; set; }
    public string? TransactionId { get; set; } 
}

public class PaymentRejectedIntegrationEvent : IntegrationEvent
{
    public long OrderNumber { get; set; }
    public string? CardBrand { get; set; }
    public string? TransactionId { get; set; }
}