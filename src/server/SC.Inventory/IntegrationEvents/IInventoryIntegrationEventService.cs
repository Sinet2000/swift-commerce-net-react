using SC.Inventory.IntegrationEvents.Events;

namespace SC.Inventory.IntegrationEvents;

public interface IInventoryIntegrationEventService
{
    Task SaveEventAndInventoryContextChangesAsync(IntegrationEvent evt);
    Task PublishThroughEventBusAsync(IntegrationEvent evt);
}
