namespace SC.Inventory.EventBusRabbitMQ;

public class EventBusOptions
{
    public string SubscriptionClientName { get; set; } = null!;
    public int RetryCount { get; set; } = 10;
}
