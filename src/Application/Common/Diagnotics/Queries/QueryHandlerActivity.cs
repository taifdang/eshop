using System.Diagnostics;

namespace Application.Common.Diagnotics.Queries;

public class QueryHandlerActivity
{
     public async Task<TResult> Execute<TQuery, TResult>(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken cancellationToken)
        where TQuery : notnull
    {
        var queryName = typeof(TQuery).Name;

        using var activity = ActivitySourceProvider.Instance.StartActivity(
            queryName,
            ActivityKind.Internal);

        activity?.SetTag(TelemetryTags.Tracing.Queries.Query, queryName);
        activity?.SetTag(TelemetryTags.Tracing.Queries.QueryType, typeof(TQuery).FullName);

        return await action(cancellationToken);
    }
}
