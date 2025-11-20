using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediatr;

    public TransactionBehavior(
        ILogger<TransactionBehavior<TRequest, TResponse>> logger, 
        IUnitOfWork uow,
        IMediator mediatr)
    {
        _logger = logger;
        _uow = uow;
        _mediatr = mediatr;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
           "{Prefix} Handled command {MediatrRequest}",
           nameof(TransactionBehavior<TRequest, TResponse>),
           typeof(TRequest).Name);

        var response = await next();

        _logger.LogInformation(
            "{Prefix} Executed the {MediatrRequest} request",
            nameof(TransactionBehavior<TRequest, TResponse>),
            typeof(TRequest).FullName);

        while (true)
        {
            await _uow.ExecuteTransactionalAsync(cancellationToken);
        }

    }
}
