using System.Windows.Media.Imaging;

namespace Study_Step.Models
{
    public class Chat
    {
        public int Id { get; set; }  // Уникальный идентификатор чата
        public string? Name { get; set; }  // Название чата (для группового чата)
        public byte[]? ContactPhoto { get; set; }
        public BitmapImage? bitmapPhoto { get; set; }
        public string? Message { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public ChatType Type { get; set; }  // Тип чата (личный, групповой)

        public ICollection<UserChat>? UserChats { get; set; } // Участники чата
        public ICollection<Message>? Messages { get; set; } // сообщения, имеющие отношение к конкретному чату
    }

    public enum ChatType
    {
        Individual,  // Личный чат
        Group     // Групповой чат
    }
}