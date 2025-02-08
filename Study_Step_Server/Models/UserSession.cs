using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Study_Step_Server.Models
{
    public class UserSession
    {
        public int UserId { get; set; }  // Идентификатор пользователя
        public string? Token { get; set; }  // JWT токен для авторизации
        public DateTime Expiration { get; set; }  // Время истечения срока действия токена
    }

    internal class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {

        }
    }
}
