using System;

namespace StockService.Event.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string message, Exception ex) :base(message, ex)
        {
        
        }
    }
}