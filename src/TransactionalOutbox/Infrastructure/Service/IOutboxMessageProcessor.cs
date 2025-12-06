namespace TransactionalOutbox.Infrastructure.Service;

public interface IOutboxMessageProcessor
{
    Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken = default);
}
