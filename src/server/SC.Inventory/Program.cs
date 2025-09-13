using SC.Inventory;
using SC.Inventory.Extensions;
using SC.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureInfrastructure();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

var withApiVersioning = builder.Services.AddApiVersioning();

builder.AddDefaultOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseStatusCodePages();

app.MapInventoryApi();

app.UseDefaultOpenApi();
app.Run();
