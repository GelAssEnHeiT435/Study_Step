using System.Windows.Media.Imaging;

namespace Study_Step.Models
{
    public class Chat
    {
        public int ChatId { get; set; }  // Уникальный идентификатор чата
        public string? Name { get; set; }  // Название чата (для группового чата)
        public BitmapImage? bitmapPhoto { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public ChatType Type { get; set; }  // Тип чата (личный, групповой)

        public int? UserId_InChat { get; set; }
    }

    public enum ChatType
    {
        Individual,  // Личный чат
        Group     // Групповой чат
    }
}