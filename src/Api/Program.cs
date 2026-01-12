using Api;
using Api.Endpoints;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddApplicationServices();
builder.AddInfrastructure();
builder.AddWebDependencies();

var app = builder.Build();

app.UseInfrastructure();
app.UseWebDependencies();

app.MapCatalogApi();
app.MapBasketApi();
app.MapOrderApi();
app.MapCustomerApi();
app.MapIdentityApi();
app.MapPaymentApi();

app.Run();

public partial class Program { }