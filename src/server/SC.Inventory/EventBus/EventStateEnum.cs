namespace SC.Inventory.Utilities;

public enum EventStateEnum : byte
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    PublishedFailed = 3
}
