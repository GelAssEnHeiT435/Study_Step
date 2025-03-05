using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Study_Step_Server.Hubs;
using Study_Step_Server.Models;
using Study_Step_Server.Services;
using Study_Step_Server.Data;
using Study_Step_Server.Repositories;
using Study_Step_Server.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;

namespace Study_Step_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection to Database PostgreSQL
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<AuthService>(); // Add Service Authorization
            

            // Add user provider to convert connectionId to UserId 
            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            

            // Add Service Authorization with JWT-Token
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

                            // Get Path from http request's body
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            // Add SignalR
            builder.Services.AddSignalR();   
            
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUserChatRepository, UserChatRepository>();
            builder.Services.AddScoped<IUoW, UoW>();

            // Add converter models to data transfer object
            builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<DtoConverterService>();

            // Add services to the container.
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapPost("/login", async (AuthUser loginModel) =>
            {
                IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    ApplicationContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    AuthService authService = scope.ServiceProvider.GetRequiredService<AuthService>();
                    
                    AuthUser? user = await dbContext.AuthorizationUsers.FirstOrDefaultAsync(users => users.Email == loginModel.Email);

                    if (!authService.VerifyPassword(user?.Password, loginModel.Password)) return Results.Unauthorized();

                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())  // TODO - ������� ������ ConnectionId � UserId
                    };
                    
                    var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            claims: claims,
                            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

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
                    AuthService authService = scope.ServiceProvider.GetRequiredService<AuthService>();
                    
                    AuthUser? user = await dbContext.AuthorizationUsers.FirstOrDefaultAsync(users => users.Email == loginModel.Email);
                    if (user != null) return Results.Unauthorized();
                    
                    string password = authService.HashPassword(loginModel.Password);
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

                    var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            claims: claims,
                            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    
                    var response = new
                    {
                        id = newuser.Id,
                        name = newuser.Name,
                        access_token = encodedJwt
                    };

                    return Results.Json(response);
                }
            });

            
            
            app.MapHub<ChatHub>("/chathub");

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}