using Microsoft.EntityFrameworkCore;
using TransactionalOutbox.Abstractions;
using TransactionalOutbox.Infrastructure.Data;

namespace TransactionalOutbox.Infrastructure
{
    public class PollingOutboxMessageRepository(PollingOutboxMessageRepositoryOptions options, OutboxDbContext dbContext) : IPollingOutboxMessageRepository
    {
        public async Task AddAsync(PollingOutboxMessage message)
        {
            await dbContext.AddAsync(message);
        }

        public async Task<IEnumerable<PollingOutboxMessage>> GetUnprocessedMessagesAsync()
        {
            return await dbContext.PollingOutboxMessages.Where(x => x.ProcessedDate == null && x.RetryCount < options.MaxRetries).ToListAsync();
        }

        public Task MarkEventAsFailedAsync(PollingOutboxMessage message, bool recoverable = true)
        {
            if (recoverable)
            {
                message.RetryCount++;
            }
            else
            {
                message.RetryCount = options.MaxRetries;
            }

            return Task.CompletedTask;
        }

        public Task MarkEventAsProcessedAsync(PollingOutboxMessage message)
        {
            message.RetryCount++;
            message.ProcessedDate = DateTime.UtcNow;

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
