using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Study_Step_Server.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Уникальный идентификатор сообщения
        public int SenderId { get; set; }  // Идентификатор отправителя
        public int ChatId { get; set; }  // Идентификатор чата
        public string? Text { get; set; }  // Текст сообщения
        public DateTime SentAt { get; set; }  // Время отправки
        public MessageType Type { get; set; }  // Тип сообщения (текст, изображение, файл)
        public string? FileUrl { get; set; }  // URL для файлов (если есть)

        public virtual User Sender { get; set; }
        public virtual Chat Chat { get; set; }
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
