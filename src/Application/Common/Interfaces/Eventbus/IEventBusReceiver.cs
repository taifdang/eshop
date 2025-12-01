namespace Application.Common.Interfaces.Eventbus;

public interface IEventBusReceiver<TConsumer, T>
{
    Task ReceiveAsync(Func<T, CancellationToken, Task> action,CancellationToken cancellationToken = default);
}