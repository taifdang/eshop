namespace Domain.Common;

public interface IAggregate<T> : IAggregate, IEntity<T>
{

}
public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvents();
}
