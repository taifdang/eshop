namespace Outbox.EF.Infrastructure.Services;

public interface IOutboxMessageProcessor
{
    Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken = default);
}
