using EventBus.Events;

namespace Contracts.IntegrationEvents;

public class UserCreatedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = default!;
}
