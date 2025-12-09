var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Api>("apiservice");

builder.Build().Run();
