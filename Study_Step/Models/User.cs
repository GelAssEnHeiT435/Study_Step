using System.Windows.Media.Imaging;

namespace Study_Step.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public BitmapImage Photo { get; set; }
        public string Email { get; set; }
        public UserStatus? Status { get; set; }  // Статус пользователя (онлайн, оффлайн)
        public DateTime? LastLogin { get; set; }  // Время последнего входа

        public ICollection<UserChat>? UserChats { get; set; }
        public ICollection<Message>? Messages { get; set; }
        
    }

    public enum UserStatus
    {
        Online = 0, // в сети
        Offline = 1, // не в сети
        Away = 2, // ожидание
        DoNotDisturb = 3 // не беспокоить
    }
}
