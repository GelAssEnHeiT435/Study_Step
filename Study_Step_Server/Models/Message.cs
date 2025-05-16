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
        public int MessageId { get; set; }  // Уникальный идентификатор сообщения
        public int? UserId { get; set; }  // Идентификатор отправителя
        public int? ChatId { get; set; }  // Идентификатор чата
        public string? Text { get; set; }  // Текст сообщения
        public DateTime? SentAt { get; set; }  // Время отправки

        public User Sender { get; set; }
        public Chat Chat { get; set; }
        public ICollection<FileModel>? Files { get; set; }
        public ICollection<MessageRead>? ReadByUsers { get; set; }
    }

    internal class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

        }
    }
}
