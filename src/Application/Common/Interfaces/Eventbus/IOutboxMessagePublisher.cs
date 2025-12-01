namespace Application.Common.Interfaces.Eventbus;

public interface IOutboxMessagePublisher
{
    Task HandleAsync(OutboxMessageData outbox,CancellationToken cancellationToken = default);
    Task SendAsync<T>(T message, CancellationToken cancellationToken = default);
}
public class OutboxMessageData
{
    public string Id { get; set; }
    public string EventType { get; set; }
    public string Payload { get; set; }
    public bool IsPublished { get; set; }   
}