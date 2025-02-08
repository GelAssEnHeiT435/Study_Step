using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Study_Step_Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? ContactPhoto { get; set; }
        public string Email { get; set; }
        public UserStatus? Status { get; set; }  // Статус пользователя (онлайн, оффлайн)
        public DateTime? LastLogin { get; set; }  // Время последнего входа

        public virtual ICollection<UserChat> UserChats { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public byte[] GetContactPhotoAsBytes()
        {
            if (string.IsNullOrEmpty(ContactPhoto) || !File.Exists(ContactPhoto))
            {
                return null;  // Возвращаем null, если путь пустой или файл не существует
            }

            return File.ReadAllBytes(ContactPhoto);  // Читаем файл в массив байтов
        }

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
