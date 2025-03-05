using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Study_Step_Server.Models
{
    public class AuthUser
    {
        public int Id { get; set; } 
        public string Name { get; set; } // Имя пользователя
        public string Email { get; set; } // Почта пользователя
        public string Password { get; set; } // Хэшированное значение пароля в БД

    }

    internal class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure(EntityTypeBuilder<AuthUser> builder)
        {
            // TODO: CheckContrains Email
        }
    }

}
