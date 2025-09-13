using SC.Inventory.IntegrationEvents.Events;

namespace SC.Inventory.Utilities;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event);
}

