using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Study_Step_Server.Models
{
    public class Message
    {
        public int MessageId { get; set; }  // Уникальный идентификатор сообщения
        public int? UserId { get; set; }  // Идентификатор отправителя
        public int? ChatId { get; set; }  // Идентификатор чата
        public string? Text { get; set; }  // Текст сообщения
        public DateTime? SentAt { get; set; }  // Время отправки
        public MessageType Type { get; set; }  // Тип сообщения (текст, изображение, файл)
        public string? FileUrl { get; set; }  // URL для файлов (если есть)

        public User Sender { get; set; }
        public Chat Chat { get; set; }
    }

    internal class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

        }
    }

    public enum MessageType
    {
        Text,       // Текстовое сообщение
        Image,      // Сообщение с изображением
        File,       // Сообщение с файлом
        Sticker     // Сообщение с стикером
    }
}
