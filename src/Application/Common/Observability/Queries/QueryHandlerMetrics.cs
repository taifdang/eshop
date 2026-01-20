using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Application.Common.Observability.Queries;

public class QueryHandlerMetrics
{
    private readonly UpDownCounter<long> _activeQueriesCounter;
    private readonly Counter<long> _totalQueriesNumber;
    private readonly Histogram<double> _handlerDuration;
    private readonly Meter _meter;

    private Stopwatch _timer;

    public QueryHandlerMetrics(IMeterFactory meterFactory)
    {
        _meter = meterFactory.Create(ActivitySourceProvider.DefaultSourceName);

        _activeQueriesCounter = _meter.CreateUpDownCounter<long>(
            TelemetryTags.Metrics.Queries.ActiveCount,
            unit: "{active_query}",
            description: "Number of queries currently being handled");

        _totalQueriesNumber = _meter.CreateCounter<long>(
            TelemetryTags.Metrics.Queries.TotalExecutedCount,
            unit: "{total_queries}",
            description: "Total number of executed query that sent to query handlers"
        );

        _handlerDuration = _meter.CreateHistogram<double>(
            TelemetryTags.Metrics.Queries.HandlerDuration,
            unit: "s",
            description: "Measures the duration of query handler");
    }

    public void StartHandling<TQuery>()
    {
        var queryName = typeof(TQuery).Name;

        var tags = new TagList {
            { TelemetryTags.Tracing.Queries.Query, queryName },
            { TelemetryTags.Tracing.Queries.QueryType, typeof(TQuery).FullName },
        };

        if (_activeQueriesCounter.Enabled)
        {
            _activeQueriesCounter.Add(1, tags);
        }

        if (_totalQueriesNumber.Enabled)
        {
            _totalQueriesNumber.Add(1, tags);
        }

        _timer = Stopwatch.StartNew();
    }

    public void StopHandling<TQuery>()
    {
         var queryName = typeof(TQuery).Name;

        var tags = new TagList {
            { TelemetryTags.Tracing.Queries.Query, queryName },
            { TelemetryTags.Tracing.Queries.QueryType, typeof(TQuery).FullName },
        };

        if (_totalQueriesNumber.Enabled)
        {
            _totalQueriesNumber.Add(-1, tags);
        }

        if(!_handlerDuration.Enabled)
        {
            return;
        }

        var elapsedTimeSeconds = _timer.Elapsed.TotalSeconds;

        _handlerDuration.Record(elapsedTimeSeconds, tags);
    }
}