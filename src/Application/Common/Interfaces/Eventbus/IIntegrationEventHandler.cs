namespace Application.Common.Interfaces.Eventbus;

//public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
//    where TIntegrationEvent : IntegrationEvent
//{
//    Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken);

//    Task IIntegrationEventHandler.HandleAsync(IntegrationEvent @event, CancellationToken cancellationToken) => Handle((TIntegrationEvent)@event, cancellationToken);
//}

//public interface IIntegrationEventHandler
//{
//    Task HandleAsync(IntegrationEvent @event, CancellationToken cancellationToken);
//}

public interface IIntegrationEventHandler<in TEvent>
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
