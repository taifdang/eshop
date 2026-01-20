using System.Diagnostics;

namespace Application.Common.Observability.Commands;

public class CommandHandlerActivity
{
     public async Task<TResult> Execute<TCommand, TResult>(
        Func<CancellationToken, Task<TResult>> action,
        CancellationToken cancellationToken)
        where TCommand : notnull
    {
        var commandName  = typeof(TCommand).Name;

        using var activity = ActivitySourceProvider.Instance.StartActivity(
            commandName,
            ActivityKind.Internal);

        activity?.SetTag(TelemetryTags.Tracing.Commands.Command, commandName);
        activity?.SetTag(TelemetryTags.Tracing.Commands.CommandType, typeof(TCommand).FullName);

        return await action(cancellationToken);
    }
}
