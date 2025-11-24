using Microsoft.AspNetCore.Builder;
using NotificationService.API.Middleware;

namespace NotificationService.API.Extensions
{
    public static class LoggingExtension
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}