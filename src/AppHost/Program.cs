//ref: https://devblogs.microsoft.com/dotnet/new-aspire-app-with-react/
//ref: https://aspire.dev/fundamentals/networking-overview/#ports-and-proxies
//ref: https://learn.microsoft.com/en-us/dotnet/aspire/architecture/overview

var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("rabbitmq");

var postgres = builder.AddPostgres("postgres").WithPgWeb();

var database = postgres.AddDatabase("shopdb");

var apiService = builder.AddProject<Projects.Api>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WithReference(rabbitmq)
    .WaitFor(database)
    .WaitFor(rabbitmq);

var identityService = builder.AddProject<Projects.IdentityService>("identityservice")
    .WithExternalHttpEndpoints()
    .WithReference(database)
    .WithReference(rabbitmq)
    .WaitFor(database)
    .WaitFor(rabbitmq)
    .WaitFor(apiService);

// test / dev local frontend with react and vite
var reactVite = builder.AddViteApp("webfrontend", "../Web/ClientApp")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEndpoint("http", e => e.Port = 3000) // fixed port for frontend
    .WithEnvironment("BROWSER", "none");

builder.AddProject<Projects.Bff>("bff")
    .WaitFor(identityService);

builder.Build().Run();
