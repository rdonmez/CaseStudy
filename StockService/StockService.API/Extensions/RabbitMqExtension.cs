using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockService.API.Middleware;
using StockService.Event;

namespace StockService.API.Extensions
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