namespace Study_Step.Models
{
    public class Message
    {
        public int SenderId { get; set; }  // Идентификатор отправителя
        public int ChatId { get; set; }  // Идентификатор чата
        public string? Text { get; set; }  // Текст сообщения
        public DateTime SentAt { get; set; }  // Время отправки
        public MessageType Type { get; set; }  // Тип сообщения (текст, изображение, файл)
        public string? FileUrl { get; set; }  // URL для файлов (если есть)


        public bool? IsOutside { get; set; }
        //public User? Sender { get; set; }
        //public Chat? Chat { get; set; }
    }
    public enum MessageType
    {
        Text,       // Текстовое сообщение
        Image,      // Сообщение с изображением
        File,       // Сообщение с файлом
        Sticker     // Сообщение с стикером
    }
}
