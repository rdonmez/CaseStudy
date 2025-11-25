namespace OrderService.Event
{
    public class Constant
    {
        public const string OrderQueue = "order_queue";
        public const string StockQueue = "stock_queue";
        public const string NotificationQueue = "notification_queue";
        public const string OrderCreatedRoutingKey = "order.created";
        public const string OrderApprovedRoutingKey = "order.approved";
        public const string OrderCanceledRoutingKey = "order.canceled";
    }
}