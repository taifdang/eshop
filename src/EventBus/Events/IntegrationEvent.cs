using MediatR;

namespace EventBus.Events;

public abstract class IntegrationEvent : INotification
{
    public Guid EventId { get; private set; }
    public DateTime CreateDate { get; private set; }

    public IntegrationEvent()
    {
        EventId = Guid.CreateVersion7();
        CreateDate = DateTime.UtcNow;
    }
}
