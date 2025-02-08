using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Study_Step_Server.Services
{
    public class JwtTokenService
    {
        private const string SecretKey = "jBmFz3nReAAmz59p9PymYHZKMHahZZQT"; // Используйте длинный и случайный ключ
        private const string Issuer = "Study_Step"; // Имя приложения или сервера

        public string GenerateJwtToken(int userId, string userEmail)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),  // Преобразуем целочисленный userId в строку
                new Claim(ClaimTypes.Email, userEmail),
            }),
                Expires = DateTime.Now.AddHours(1), // Токен истекает через 1 час
                Issuer = Issuer,
                Audience = Issuer,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
