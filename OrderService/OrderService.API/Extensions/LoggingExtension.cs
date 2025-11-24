using Microsoft.AspNetCore.Builder;
using OrderService.API.Middleware;

namespace OrderService.API.Extensions
{
    public static class LoggingExtension
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}