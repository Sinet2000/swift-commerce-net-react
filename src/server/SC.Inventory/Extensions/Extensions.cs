using Microsoft.EntityFrameworkCore;
using SC.Inventory.EventBus;
using SC.Inventory.EventBusRabbitMQ;
using SC.Inventory.Infrastructure;
using SC.Inventory.IntegrationEvents;
using SC.Inventory.IntegrationEvents.EventHandling;
using SC.Inventory.IntegrationEvents.Events;
using SC.Inventory.Utilities;

namespace SC.Inventory.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // Avoid loading full database config and migrations if startup
        // is being invoked from build-time OpenAPI generation
        if (builder.Environment.IsBuild())
        {
            builder.Services.AddDbContext<InventoryContext>();
            return;
        }

        builder.AddNpgsqlDbContext<InventoryContext>("inventorydb", configureDbContextOptions: dbContextOptionsBuilder =>
        {
            dbContextOptionsBuilder.UseNpgsql();
        });

        // REVIEW: This is done for development ease but shouldn't be here in production
        builder.Services.AddMigration<InventoryContext, InventoryContextSeed>();

        // Add the integration services that consume the DbContext
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<InventoryContext>>();

        builder.Services.AddTransient<IInventoryIntegrationEventService, InventoryIntegrationEventService>();

        builder.AddRabbitMqEventBus("eventbus")
               .AddSubscription<ConfirmedInventoryStockItemEvent, ConfirmedInventoryStockItemEventHandler>();

        builder.Services.AddOptions<InventoryOptions>()
            .BindConfiguration(nameof(InventoryOptions));

        // if (builder.Configuration["OllamaEnabled"] is string ollamaEnabled && bool.Parse(ollamaEnabled))
        // {
        //     builder.AddOllamaApiClient("embedding")
        //         .AddEmbeddingGenerator();
        // }
        // else if (!string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("textEmbeddingModel")))
        // {
        //     builder.AddOpenAIClientFromConfiguration("textEmbeddingModel")
        //         .AddEmbeddingGenerator();
        // }

        // builder.Services.AddScoped<ICatalogAI, CatalogAI>();
    }
}

