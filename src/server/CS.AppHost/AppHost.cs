using CS.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var redis = builder.AddRedis("redis");
var rabbitMq = builder.AddRabbitMQ("eventbus")
    .WithLifetime(ContainerLifetime.Persistent);

var postgres = builder.AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var inventoryDB = postgres.AddDatabase("inventorydb");

var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

var inventoryApi = builder.AddProject<Projects.CS_Inventory>("inventory-api", launchProfileName)
    .WithReference(rabbitMq).WaitFor(rabbitMq)
    .WithReference(inventoryDB);

// Reverse proxies
// builder.AddYarp("mobile-bff")
//     .WithExternalHttpEndpoints()
//     .ConfigureMobileBffRoutes(catalogApi, orderingApi, identityApi);

// var webApp = builder.AddProject<Projects.WebApp>("webapp", launchProfileName)
//     .WithExternalHttpEndpoints()
//     .WithUrls(c => c.Urls.ForEach(u => u.DisplayText = $"Online Store ({u.Endpoint?.EndpointName})"))
//     .WithReference(basketApi)
//     .WithReference(catalogApi)
//     .WithReference(orderingApi)
//     .WithReference(rabbitMq).WaitFor(rabbitMq)
//     .WithEnvironment("IdentityUrl", identityEndpoint);

builder.Build().Run();

static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "SC_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}