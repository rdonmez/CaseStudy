using System;

namespace NotificationService.Event.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string message, Exception ex) :base(message, ex)
        {
        
        }
    }
}