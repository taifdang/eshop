using Api;
using Api.Endpoints;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddApplicationServices();
builder.AddInfrastructure();
builder.AddWebServices();

var app = builder.Build();

app.UseInfrastructure();
app.UseWebServices();

app.MapCatalogApi();
app.MapBasketApi();
app.MapOrderApi();
app.MapCustomerApi();
app.MapIdentityApi();

app.Run();

public partial class Program { }