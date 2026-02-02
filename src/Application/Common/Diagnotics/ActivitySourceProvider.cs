using System.Diagnostics;

namespace Application.Common.Diagnotics;

public static class ActivitySourceProvider
{
    public const string DefaultSourceName = "eshop";

    public static readonly ActivitySource Instance = new(DefaultSourceName, "v1");
}