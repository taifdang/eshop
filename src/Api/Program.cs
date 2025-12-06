using Api;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplication();
builder.AddInfrastructure();
builder.AddServiceDefaults();

var app = builder.Build();


app.UseInfrastructure();
app.UseServiceDefaults();

app.Run();

public partial class Program { }