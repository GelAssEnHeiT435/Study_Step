using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Study_Step_Server.Models
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatId { get; set; }  // Уникальный идентификатор чата
        public string? Name { get; set; }  // Название чата (для группового чата)
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public ChatType? Type { get; set; }  // Тип чата (личный, групповой)

        public ICollection<UserChat> UserChats { get; set; } // Участники чата
        public ICollection<Message>? Messages { get; set; } // сообщения, имеющие отношение к конкретному чату
        public ICollection<DeletedMessage>? DeletedMessages { get; set; }
        public ICollection<DeletedChat>? DeletedChats { get; set; }
    }

    internal class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {

        }
    }

    public enum ChatType
    {
        Individual,  // Личный чат
        Group     // Групповой чат
    }
}