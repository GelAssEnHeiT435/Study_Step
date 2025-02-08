using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Study_Step_Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            System.Diagnostics.Debug.WriteLine("запуск");
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");  // Включение CORS

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
