using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Shared.Constants;

namespace Infrastructure.HealthCheck;

public static class Extensions
{
    private const string HealthEndpoint = "/healthz";
    private const string AliveEndpoint = "/alive";

    public static IHostApplicationBuilder AddCustomHealthCheck(this IHostApplicationBuilder builder)
    {
        var healthOptions = builder.Configuration.GetSection("HealthOptions").Get<HealthOptions>()!;

        if (healthOptions.Enabled)
        {
            var appOptions = builder.Configuration.GetSection("AppOptions").Get<AppOptions>()!;
            var postgreOptions = builder.Configuration.GetSection("ConnectionStrings:shopdb").Value;

            if (!string.IsNullOrEmpty(postgreOptions))
            {
                builder.Services.AddHealthChecks().AddNpgSql(postgreOptions);
            }

            builder.Services.AddHealthChecksUI(cfg =>
            {
                cfg.SetEvaluationTimeInSeconds(60); // time in seconds between check
                cfg.AddHealthCheckEndpoint($"Self Check - {appOptions.Name}", HealthEndpoint);
            }).AddInMemoryStorage();
        }

        return builder;
    }

    public static WebApplication UseCustomHealthCheck(this WebApplication app)
    {
        var healthOptions = app.Configuration.GetSection("HealthOptions").Get<HealthOptions>()!;

        if (app.Environment.IsDevelopment())
        {
            app.MapHealthChecks(HealthEndpoint, new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapHealthChecks(AliveEndpoint, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live"),
            });
        }

        if (healthOptions.Enabled)
        {          
            app.MapHealthChecksUI(opt => opt.UIPath = "/health-ui");
        }

        return app;
    }
}
