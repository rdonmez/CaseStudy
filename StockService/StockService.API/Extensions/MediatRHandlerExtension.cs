using Microsoft.Extensions.DependencyInjection;

namespace StockService.API.Extensions
{
    public static class MediatRHandlerExtension
    {
        public static IServiceCollection RegisterRequestHandlers(
            this IServiceCollection services)
        {
            return services
                .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(MediatRHandlerExtension).Assembly));
        }
    }
}