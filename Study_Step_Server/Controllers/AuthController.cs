using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Models;
using Study_Step_Server.Services;

namespace Study_Step_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly AuthService _authService;
        //private readonly JwtTokenService _jwtTokenService;

        public AuthController(ApplicationContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
            //_jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostAuthorization([FromBody] AuthUser AuthUser)
        {
            // проверка на пустоту передаваемого объекта
            if (AuthUser == null || string.IsNullOrEmpty(AuthUser.Email) || string.IsNullOrEmpty(AuthUser.Password))
            {
                return BadRequest("Пожалуйста, введите имя пользователя и пароль.");
            }

            // Поиск пользователя по его логину
            AuthUser? result = await _context.AuthorizationUsers.SingleOrDefaultAsync(users => users.Email == AuthUser.Email);

            // Проверка пользователя на наличие
            if (result == null)
            {
                return Unauthorized("Неверное имя пользователя или пароль.");
            }

            // Проверка соответствия паролей
            if (!_authService.VerifyPassword(result.Password, AuthUser.Password))
            {
                return Unauthorized("Неверное имя пользователя или пароль.");
            }

            //// Генерация JWT токена
            //var token = _jwtTokenService.GenerateJwtToken(result.Id, result.Email);
            //UserSession userSession = new UserSession
            //{
            //    UserId = result.Id,
            //    Token = token,
            //    Expiration = DateTime.UtcNow.AddMinutes(60) // Пример времени истечения срока действия токена
            //};
            //Console.WriteLine(token);

            // Отправка токена в ответе
            return Ok(new { message = "Ok", Id = result.Id, Name = result.Name });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthUser AuthUser)
        {
            Console.WriteLine($"Name: {AuthUser.Name}, Login: {AuthUser.Email}, Password: {AuthUser.Password}");
            // Проверка на заполненность данных
            if (AuthUser == null || string.IsNullOrEmpty(AuthUser.Name) || 
                                    string.IsNullOrEmpty(AuthUser.Email) || 
                                    string.IsNullOrEmpty(AuthUser.Password))
            {
                return BadRequest("Пожалуйста, введите имя пользователя, логин и пароль.");
            }

            // Поиск пользователей с таким же логином. Если уже есть, то отправляет соответствующее сообщение
            AuthUser? existingUser = await _context.AuthorizationUsers.SingleOrDefaultAsync(user => user.Email == AuthUser.Email);

            if (existingUser != null)
            {
                return BadRequest("Пользователь с таким именем уже существует.");
            }

            // Хэширование пароля и добавление пользователя в БД
            string password = _authService.HashPassword(AuthUser.Password);
            AuthUser auser = new AuthUser 
            {
                Name = AuthUser.Name,
                Email = AuthUser.Email, 
                Password = password 
            };
            User user = new User
            {
                Username = AuthUser.Name,
                Email = AuthUser.Email,
            };

            _context.AuthorizationUsers.Add(auser);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Поиск пользователя по его логину
            AuthUser? result = await _context.AuthorizationUsers.SingleOrDefaultAsync(users => users.Email == AuthUser.Email);

            //// Генерация JWT токена для нового пользователя с целочисленным Id
            //var token = _jwtTokenService.GenerateJwtToken(result.Id, auser.Email);
            //UserSession userSession = new UserSession
            //{
            //    UserId = result.Id,
            //    Token = token,
            //    Expiration = DateTime.UtcNow.AddMinutes(60) // Пример времени истечения срока действия токена
            //};
            //Console.WriteLine(token);

            // Отправка токена в ответе
            return Ok( new { message = "Ok", Id = result.Id, Name = result.Name } );
        }
    }
}