using Microsoft.AspNetCore.Builder;
using StockService.API.Middleware;

namespace StockService.API.Extensions
{
    public static class ErrorHandlerExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}