using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SC.Inventory.Infrastructure;
using SC.Inventory.IntegrationEvents;

namespace SC.Inventory.Services;

public class InventoryService(
    InventoryContext context,
    IOptions<InventoryOptions> options,
    ILogger<InventoryService> logger,
    [FromServices] IInventoryIntegrationEventService eventService
)
{
    public InventoryContext Context { get; } = context;
    public IOptions<InventoryOptions> Options { get; } = options;
    public ILogger<InventoryService> Logger { get; } = logger;
    public IInventoryIntegrationEventService EventService { get; } = eventService;
}
