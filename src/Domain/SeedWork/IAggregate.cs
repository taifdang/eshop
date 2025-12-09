using Domain.Events;

namespace Domain.SeedWork;

public interface IAggregate<T> : IAggregate, IEntity<T>
{
}

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent @event);
    void ClearDomainEvents();
}