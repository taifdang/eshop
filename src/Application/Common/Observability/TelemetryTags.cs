namespace Application.Common.Observability;

public static class TelemetryTags
{
    public static class Tracing
    {
        public static class Commands
        {
            public static string Command = $"{ActivitySourceProvider.DefaultSourceName}.command";
            public static string CommandType = $"{Command}.type";
            public static string CommandHandler = $"{Command}.handler";
            public static string CommandHandlerType = $"{CommandHandler}.type";
        }

        public static class Queries
        {
            public static string Query = $"{ActivitySourceProvider.DefaultSourceName}.query";
            public static string QueryType = $"{Query}.type";
            public static string QueryHandler = $"{Query}.handler";
            public static string QueryHandlerType = $"{QueryHandler}.type";
        }
    }

    public static class Metrics
    {
        public static class Commands
        {
            public static string Command = $"{ActivitySourceProvider.DefaultSourceName}.command";
            public static string CommandType = $"{Command}.type";
            public static string CommandHandler = $"{Command}.handler";
            public static string ActiveCount = $"{CommandHandler}.active.count";
            public static string TotalExecutedCount = $"{CommandHandler}.total.count";
            public static string HandlerDuration = $"{CommandHandler}.duration";
        }

        public static class Queries
        {
            public static string Query = $"{ActivitySourceProvider.DefaultSourceName}.query";
            public static string QueryType = $"{Query}.type";
            public static string QueryHandler = $"{Query}.handler";
            public static string ActiveCount = $"{QueryHandler}.active.count";
            public static string TotalExecutedCount = $"{QueryHandler}.total.count";
            public static string HandlerDuration = $"{QueryHandler}.duration";
        }
    }
}
