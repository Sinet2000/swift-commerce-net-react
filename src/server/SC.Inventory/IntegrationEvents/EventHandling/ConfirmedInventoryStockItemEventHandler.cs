using SC.Inventory.Infrastructure;
using SC.Inventory.IntegrationEvents.Events;
using SC.Inventory.Utilities;

namespace SC.Inventory.IntegrationEvents.EventHandling;

public class ConfirmedInventoryStockItemEventHandler(
    InventoryContext dbContext,
    ILogger<ConfirmedInventoryStockItemEventHandler> logger
    ) : IIntegrationEventHandler<ConfirmedInventoryStockItemEvent>
{
    public async Task Handle(ConfirmedInventoryStockItemEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        var item = dbContext.InventoryItems.Find(@event.ItemID);
        item?.RemoveStock(1);

        await dbContext.SaveChangesAsync();
    }
}
