using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Study_Step_Server.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? ContactPhoto { get; set; }
        public string Email { get; set; }
        public UserStatus? Status { get; set; }  // Статус пользователя (онлайн, оффлайн)
        public DateTime? LastLogin { get; set; }  // Время последнего входа

        public ICollection<UserChat>? UserChats { get; set; }
        public ICollection<Message>? Messages { get; set; }

    }
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

        }
    }

    public enum UserStatus
    {
        Online,
        Offline,
        Away,
        DoNotDisturb
    }
}
