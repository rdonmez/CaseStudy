using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StockService.Entity;
using StockService.Entity.Repositories;
using StockService.Event;
using StockService.API.Extensions; 

namespace StockService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        { 
            services.UseRabbitMq(Configuration);
            services.AddDbContext<StockDbContext>(options => 
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
            );
             
            services.AddScoped<IStockRepository, StockRepository>(); 
            services.AddHostedService<StockEventService>();
            services.RegisterRequestHandlers();
            services.AddControllers();
 
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo{
                    Version = "v1",
                    Title = "StockService API"
                });
            });
            
            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<StockDbContext>().Database.Migrate();
                scope.ServiceProvider.GetService<IStockRepository>();
            } 
            
            app.UseErrorHandlingMiddleware();
            app.UseRequestLogging(); 
            app.UseSwagger();
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }

            app.UseHttpsRedirection(); 
            app.UseRouting(); 
            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    }
}