using Domain.Events;

namespace Domain.SeedWork;

public abstract class Aggregate<T> : Entity<T>, IAggregate<T>
{
    private List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
