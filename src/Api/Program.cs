using Api;
using Api.Endpoints;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddApplicationServices();
builder.AddInfrastructure();
builder.AddPersistence();

var app = builder.Build();

app.MapInfrastructure();
app.MapPersistence();

app.MapCatalogApi();
app.MapBasketApi();
app.MapOrderApi();
app.MapCustomerApi();
app.MapPaymentApi();

await app.MigrateAndSeedDataAsync();

app.Run();

public partial class Program { }