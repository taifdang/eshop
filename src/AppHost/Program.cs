//ref: https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/
//ref: https://aspire.dev/fundamentals/networking-overview/#ports-and-proxies
//ref: https://learn.microsoft.com/en-us/dotnet/aspire/architecture/overview

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var postgres = builder.AddPostgres("postgres").WithPgWeb();

if (builder.ExecutionContext.IsPublishMode)
{
    postgres.WithDataVolume("postgres-data")
        .WithLifetime(ContainerLifetime.Persistent);
}

var database = postgres.AddDatabase("shopdb");

var apiService = builder.AddProject<Projects.Api>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WaitFor(database)
    .WithUrls(context =>
    {
        foreach (var url in context.Urls)
        {
            url.DisplayLocation = UrlDisplayLocation.DetailsOnly;
        }

        context.Urls.Add(new()
        {
            Url = "/openapi/swagger/index.html",
            DisplayText = "API Reference",
            Endpoint = context.GetEndpoint("https")
        });
    })
  .PublishAsDockerComposeService((_, svc) =>
  {
      // When creating the docker compose service
      svc.Restart = "always";
  });

// test / dev
var reactVite = builder.AddViteApp("webfrontend", "../Web/ClientApp")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEndpoint("http", e => e.Port = 3000) // fixed port for frontend
    .WithEnvironment("BROWSER", "none")
    .WithUrl("", "UI");

builder.Build().Run();
