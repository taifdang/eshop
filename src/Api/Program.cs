using Api;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplication();
builder.AddInfrastructure();
builder.AddServiceCollections();


var app = builder.Build();

app.UseInfrastructure();
app.UseServiceCollections();

app.Run();

public partial class Program { }