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
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;

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
            builder.Services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 1024 * 1024 * 10; 
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUserChatRepository, UserChatRepository>();
            builder.Services.AddScoped<IUoW, UoW>();

            // Add converter models to data transfer object
            builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);
            builder.Services.AddSingleton<IFileService, FileService>(provider =>
                                            new FileService("D:/my_works/dotnetProjects/Study_Step/Study_Step_Server/Media"));
            builder.Services.AddScoped<DtoConverterService>();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 МБ
            });

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
                    DtoConverterService dtoConverter = scope.ServiceProvider.GetRequiredService<DtoConverterService>();

                    AuthUser? user = await dbContext.AuthorizationUsers.FirstOrDefaultAsync(users => users.Email == loginModel.Email);
                    User? returnUser = await dbContext.Users.FirstOrDefaultAsync(users => users.Email == loginModel.Email);

                    if (user == null || !authService.VerifyPassword(user?.Password, loginModel.Password)) return Results.Unauthorized();

                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unique Token's ID
                    };
                    
                    // Generate JWT-Token for control user's activity
                    var accessToken = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(2),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), 
                                                                       SecurityAlgorithms.HmacSha256));
                    var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(accessToken);

                    // Generate RefreshToken for automatic log in
                    var refreshToken = new RefreshToken()
                    {
                        Token = authService.GenerateSecureToken(32),
                        UserId = user.Id,
                        ExpiryDate = DateTime.UtcNow.AddDays(7),
                        Created = DateTime.UtcNow
                    };
                    dbContext.RefreshTokens.Add(refreshToken);
                    dbContext.SaveChanges();

                    var response = new
                    {
                        user_object = dtoConverter.GetUserDTO(returnUser),
                        access_token = encodedAccessJwt,
                        refresh_token = refreshToken.Token,
                        expires_in = (int)TimeSpan.FromMinutes(15).TotalSeconds
                    };

                    return Results.Json(response);
                }
            });

            app.MapPost("/refresh", async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                string json = await reader.ReadToEndAsync();
                string? oldRefreshToken = JsonConvert.DeserializeObject<string>(json);

                IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    ApplicationContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    AuthService authService = scope.ServiceProvider.GetRequiredService<AuthService>();
                    DtoConverterService dtoConverter = scope.ServiceProvider.GetRequiredService<DtoConverterService>();

                    var oldToken = await dbContext.RefreshTokens.Include(rt => rt.User)
                                         .FirstOrDefaultAsync(rt => rt.Token == oldRefreshToken);
                    if (oldToken == null || oldToken.ExpiryDate < DateTime.UtcNow)
                    {
                        await dbContext.RefreshTokens
                                   .Where(rt => rt.Token == oldRefreshToken)
                                   .ExecuteDeleteAsync();
                        return Results.Unauthorized();
                    }

                    dbContext.RefreshTokens.Remove(oldToken);
                    await dbContext.SaveChangesAsync();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, oldToken.User.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unique Token's ID
                    };

                    // Generate JWT-Token for control user's activity
                    var accessToken = new JwtSecurityToken(
                            issuer: AuthOptions.ISSUER,
                            audience: AuthOptions.AUDIENCE,
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(2),
                            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                                                                       SecurityAlgorithms.HmacSha256));
                    var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(accessToken);

                    var newRefreshToken = new RefreshToken()
                    {
                        Token = authService.GenerateSecureToken(32),
                        UserId = oldToken.User.Id,
                        ExpiryDate = DateTime.UtcNow.AddDays(7),
                        Created = DateTime.UtcNow
                    };

                    dbContext.RefreshTokens.Add(newRefreshToken);
                    await dbContext.SaveChangesAsync();

                    User? returnUser = await dbContext.Users.FirstOrDefaultAsync(users => users.Email == oldToken.User.Email);

                    return Results.Ok(new
                    {
                        user_object = dtoConverter.GetUserDTO(returnUser),
                        access_token = encodedAccessJwt,
                        refresh_token = newRefreshToken.Token,
                        expires_in = 900
                    });
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

            app.MapPost("/logout", async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                string json = await reader.ReadToEndAsync();
                string? refreshToken = JsonConvert.DeserializeObject<string>(json);

                if (string.IsNullOrEmpty(refreshToken))
                    return Results.BadRequest("Token not provided");

                IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
                using (var scope = scopeFactory.CreateScope())
                {
                    ApplicationContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                    await dbContext.RefreshTokens
                                   .Where(rt => rt.Token == refreshToken)
                                   .ExecuteDeleteAsync();
                    return Results.Ok();
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