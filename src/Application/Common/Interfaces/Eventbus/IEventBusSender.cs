namespace Application.Common.Interfaces.Eventbus;

public interface IEventBusSender<T>
{
    Task SendAsync(T message, CancellationToken cancellationToken = default);
}
