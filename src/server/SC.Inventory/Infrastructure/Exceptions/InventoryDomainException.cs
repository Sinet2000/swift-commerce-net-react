namespace SC.Inventory.Infrastructure.Exceptions;

/// <summary>
/// Exception type for app exceptions
/// </summary>
public class InventoryDomainException : Exception
{
    public InventoryDomainException()
    { }

    public InventoryDomainException(string message)
        : base(message)
    { }

    public InventoryDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
