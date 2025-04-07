using BCrypt.Net;
using System.Security.Cryptography;

namespace Study_Step_Server.Services
{
    public class AuthService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string enteredPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }

        public string GenerateSecureToken(int length)
        {
            var randomBytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes)
                          .Replace("+", "-")
                          .Replace("/", "_")
                          .Replace("=", "");
        }
    }
}
