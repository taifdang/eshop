namespace Application.Common.Interfaces.Eventbus;

public interface IEventBus
{
    Task SendAsync<T>(T message, CancellationToken cancellationToken = default);
}
