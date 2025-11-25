namespace NotificationService.Event
{
    public class Constant
    {
        public const string NotificationQueue = "notification_queue";
        public const string OrderEventsRoutingKey = "order.*";
        
        public const string NotificationSendQueue = "notification_send_queue";
        public const string NotificationSendRoutingKey = "notification.send";
    }
}