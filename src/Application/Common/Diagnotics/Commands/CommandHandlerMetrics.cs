using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Application.Common.Diagnotics.Commands;

public class CommandHandlerMetrics
{
    private readonly UpDownCounter<long> _activeCommandsCounter;
    private readonly Counter<long> _totalCommandsNumber;
    private readonly Histogram<double> _handlerDuration;
    private readonly Meter _meter;

    private Stopwatch _timer;

    public CommandHandlerMetrics(IMeterFactory meterFactory)
    {
        _meter = meterFactory.Create(ActivitySourceProvider.DefaultSourceName);

        _activeCommandsCounter = _meter.CreateUpDownCounter<long>(
            TelemetryTags.Metrics.Commands.ActiveCount,
            unit: "{active_command}",
            description: "Number of commands currently being handled");

        _totalCommandsNumber = _meter.CreateCounter<long>(
            TelemetryTags.Metrics.Commands.TotalExecutedCount,
            unit: "{total_commands}",
            description: "Total number of executed command that sent to command handlers"
        );

        _handlerDuration = _meter.CreateHistogram<double>(
            TelemetryTags.Metrics.Commands.HandlerDuration,
            unit: "s",
            description: "Measures the duration of command handler");
    }

    public void StartHandling<TCommand>()
    {
        var commandName = typeof(TCommand).Name;
        var tags = new TagList { 
            { TelemetryTags.Tracing.Commands.Command, commandName },
            { TelemetryTags.Tracing.Commands.CommandType, typeof(TCommand).FullName }
        };

        if (_activeCommandsCounter.Enabled)
        {
            _activeCommandsCounter.Add(1, tags);
        }

        if (_totalCommandsNumber.Enabled)
        {
            _totalCommandsNumber.Add(1, tags);
        }

        _timer = Stopwatch.StartNew();
    }

    public void StopHandling<TCommand>()
    {
  
        var commandName = typeof(TCommand).Name;

        var tags = new TagList {
            { TelemetryTags.Tracing.Commands.Command, commandName },
            { TelemetryTags.Tracing.Commands.CommandType, typeof(TCommand).FullName },
        };

        if (_activeCommandsCounter.Enabled)
        {
            _activeCommandsCounter.Add(-1, tags);
        }

        if(!_handlerDuration.Enabled)
        {
            return;
        }

        var elapsedTimeSeconds = _timer.Elapsed.TotalSeconds;

        _handlerDuration.Record(elapsedTimeSeconds, tags);
    }

}
