using System;

namespace OrderService.Event.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string message, Exception ex) :base(message, ex)
        {
        
        }
    }
}