using Api;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplication();
builder.AddInfrastructure();
builder.AddPresentation();

var app = builder.Build();

app.UsePresentation();  
app.UseInfrastructure();

app.Run();

public partial class Program { }