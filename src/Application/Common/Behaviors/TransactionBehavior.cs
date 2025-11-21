using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IApplicationDbContext _context;

    public TransactionBehavior(
        ILogger<TransactionBehavior<TRequest, TResponse>> logger, 
        IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IRequest<TResponse>)
        {
            return await next();
        }
           
        var strategy = ((DbContext)_context).Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await ((DbContext)_context).Database.BeginTransactionAsync(IsolationLevel.ReadCommitted ,cancellationToken);
            
            var response = await next();

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return response;
        });
    }
}
