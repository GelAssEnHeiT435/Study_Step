using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Study_Step_Server.Models
{
    public class AuthUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Name { get; set; } // Имя пользователя
        public string Email { get; set; } // Почта пользователя
        public string Password { get; set; } // Хэшированное значение пароля в БД

        public List<RefreshToken> RefreshTokens { get; set; }
    }

    internal class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure(EntityTypeBuilder<AuthUser> builder)
        {
            // TODO: CheckContrains Email
        }
    }

}
