using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Event;

namespace OrderService.API.Extensions
{
    public static class RabbitMqExtension
    { 
        public static IServiceCollection UseRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitMqSection = configuration.GetSection("RabbitMQ");
            var hostname = rabbitMqSection["Hostname"];
            var username = rabbitMqSection["Username"];
            var password = rabbitMqSection["Password"];
             
            services.AddSingleton<EventManager>(sp =>
            {
                var eventManager = EventManager.CreateAsync(hostname, username, password).GetAwaiter()
                    .GetResult();
                
                eventManager.DeclareExchangeAsync().GetAwaiter().GetResult();
                
                return eventManager;
            });

            return services;
        }
    }
}