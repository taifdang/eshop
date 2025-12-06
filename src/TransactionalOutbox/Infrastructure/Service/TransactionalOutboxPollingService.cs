using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TransactionalOutbox.Infrastructure.Service;

internal class TransactionalOutboxPollingService : BackgroundService
{
    private readonly ILogger<TransactionalOutboxPollingService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TransactionalOutboxPollingService(
        ILogger<TransactionalOutboxPollingService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while(!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IOutboxMessageProcessor>();
                    await service.ProcessOutboxMessagesAsync(cancellationToken);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error occurred while processing outbox messages.");
            }
        }
    }
}
