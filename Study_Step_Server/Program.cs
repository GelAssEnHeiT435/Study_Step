using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Study_Step_Server.Hubs;
using Study_Step_Server.Models;
using Study_Step_Server.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace Study_Step_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ѕолучение строки подключени€
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // —ервис регистрации
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>(); // ”станавливаем сервис дл€ получени€ Id пользовател€

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // если запрос направлен хабу
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            {
                                // получаем токен из строки запроса
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // подключение сервисов SignalR
            builder.Services.AddSignalR();      

            // Add services to the container.
            builder.Services.AddControllers();
                

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();   // добавление middleware аутентификации 
            app.UseAuthorization();   // добавление middleware авторизации 

            app.MapPost("/login", async (AuthUser loginModel) =>
            {
                IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    ApplicationContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    AuthService _authService = scope.ServiceProvider.GetRequiredService<AuthService>();

                    // находим пользовател€ 
                    AuthUser? user = await dbContext.AuthorizationUsers.FirstOrDefaultAsync(users => users.Email == loginModel.Email);
                    // если пользователь не найден, отправл€ем статусный код 401
                    if (!_authService.VerifyPassword(user?.Password, loginModel.Password)) return Results.Unauthorized();

                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())  // TODO - сделать св€зку ConnectionId и UserId
                    };
                    // создаем JWT-токен
                    var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            claims: claims,
                            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    Console.WriteLine(user.Id);
                    // формируем ответ
                    var response = new
                    {
                        id = user.Id,
                        name = user.Name,
                        access_token = encodedJwt
                    };

                    return Results.Json(response);
                }
            });

            app.MapPost("/register", async (AuthUser loginModel) =>
            {
                IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    ApplicationContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    AuthService _authService = scope.ServiceProvider.GetRequiredService<AuthService>();

                    // находим пользовател€ 
                    AuthUser? user = await dbContext.AuthorizationUsers.FirstOrDefaultAsync(users => users.Email == loginModel.Email);
                    // если пользователь не найден, отправл€ем статусный код 401
                    if (user != null) return Results.Unauthorized();

                    // ’эширование парол€ и добавление пользовател€ в Ѕƒ
                    string password = _authService.HashPassword(loginModel.Password);
                    AuthUser auser = new AuthUser
                    {
                        Name = loginModel.Name,
                        Email = loginModel.Email,
                        Password = password
                    };
                    User person = new User
                    {
                        Username = loginModel.Name,
                        Email = loginModel.Email,
                    };

                    dbContext.AuthorizationUsers.Add(auser);
                    dbContext.Users.Add(person);
                    await dbContext.SaveChangesAsync();

                    AuthUser? newuser = await dbContext.AuthorizationUsers.FirstOrDefaultAsync(users => users.Email == loginModel.Email);

                    var claims = new List<Claim>
                    {
                        new Claim( ClaimTypes.NameIdentifier, newuser.Id.ToString() )
                    };
                    // создаем JWT-токен
                    var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            claims: claims,
                            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                    // формируем ответ
                    var response = new
                    {
                        id = newuser.Id,
                        name = newuser.Name,
                        access_token = encodedJwt
                    };

                    return Results.Json(response);
                }
            });


            app.MapHub<ChatHub>("/chathub");   // обработка запросов по пути конкретному пути

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}