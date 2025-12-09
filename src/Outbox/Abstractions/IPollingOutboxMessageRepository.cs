namespace Outbox.Abstractions;

public interface IPollingOutboxMessageRepository
{
    Task AddAsync(PollingOutboxMessage message);
    Task<IEnumerable<PollingOutboxMessage>> GetUnprocessedMessagesAsync();
    Task MarkEventAsProcessedAsync(PollingOutboxMessage message);
    Task MarkEventAsFailedAsync(PollingOutboxMessage message, bool recoverable = true);
    Task SaveChangesAsync();
}
