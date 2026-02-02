using Application.Common.Diagnotics.Commands;
using Application.Common.Diagnotics.Queries;
using MediatR;

namespace Application.Behaviors;

public class ActivityBehavior<TRequest, TResponse>(
    CommandHandlerMetrics commandMetrics,
    QueryHandlerMetrics queryMetrics,
    CommandHandlerActivity commandActivity,
    QueryHandlerActivity queryActivity) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var messageType = request.GetType().Name;
        var isCommand = messageType.ToLowerInvariant().EndsWith("Command".ToLowerInvariant());

        if (isCommand)
        {
            commandMetrics.StartHandling<TRequest>();
        }
        else
        {
            queryMetrics.StartHandling<TRequest>();
        }

        try
        {
            if (isCommand)
            {
                return await commandActivity.Execute<TRequest, TResponse>(
                    _ =>  next(),
                    cancellationToken);
            }
            else
            {
                return await queryActivity.Execute<TRequest, TResponse>(
                    _ =>  next(),
                    cancellationToken);
            }
        }
        finally
        {
            if(isCommand)
            {
                commandMetrics.StopHandling<TRequest>();
            }
            else
            {
                queryMetrics.StopHandling<TRequest>();
            }
        }
    }
}
