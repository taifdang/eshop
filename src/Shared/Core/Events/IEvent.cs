namespace Shared.Core.Events;

public interface IEvent
{
    Guid EventId => Guid.NewGuid();
    public DateTime CreateDate => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName;
}
