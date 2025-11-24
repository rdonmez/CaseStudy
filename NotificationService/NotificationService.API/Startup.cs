using System.Text.Json;
using System.Text.Json.Serialization; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Microsoft.OpenApi.Models;
using NotificationService.API.Extensions;
using NotificationService.Entity;
using NotificationService.Entity.Repositories;
using NotificationService.Event;

namespace NotificationService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.UseRabbitMq(Configuration);
            
            services.AddDbContext<NotificationDbContext>(options => 
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
            );
            
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddHostedService<Event.NotificationService>();
            services.AddHostedService<NotificationSendService>();
            services.RegisterRequestHandlers();
            services.AddControllers();
             
            services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo{
                    Version = "v1",
                    Title = "NotificationService API"
                });
            });
            
            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<NotificationDbContext>().Database.Migrate();
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandlingMiddleware();
            app.UseRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(options => 
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
            app.UseHttpsRedirection();
            app.UseRouting(); 
            app.UseAuthorization(); 
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}