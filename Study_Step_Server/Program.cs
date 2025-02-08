using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Models;
using Study_Step_Server.Services;

namespace Study_Step_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Получение строки подключения
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Регистрация сервисов
            builder.Services.AddScoped<AuthService>();

            // Добавляем сервис для генерации токенов
            //builder.Services.AddSingleton<JwtTokenService>();  

            // Add services to the container.
            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
