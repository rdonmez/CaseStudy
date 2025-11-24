using Microsoft.AspNetCore.Builder;
using StockService.API.Middleware;

namespace StockService.API.Extensions
{
    public static class LoggingExtension
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}