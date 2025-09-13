namespace SC.Inventory.IntegrationEvents.Events;

public record class ConfirmedInventoryStockItemEvent(int ItemID, bool HasStock) : IntegrationEvent;
