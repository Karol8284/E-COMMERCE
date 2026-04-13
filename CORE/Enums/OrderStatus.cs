namespace CORE.Enums
{
    /// <summary>
    /// OrderStatus - defines the possible states of an order throughout its lifecycle
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>Order has been created but not yet processed</summary>
        Pending,

        /// <summary>Order has been confirmed and is being prepared for shipment</summary>
        Confirmed,

        /// <summary>Order has been shipped and is in transit</summary>
        Shipped,

        /// <summary>Order has been successfully delivered</summary>
        Delivered,

        /// <summary>Order has been cancelled by customer or system</summary>
        Cancelled,
    }
}
