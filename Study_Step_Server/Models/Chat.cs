using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Study_Step_Server.Models
{
    public class Chat
    {
        public int ChatId { get; set; }  // Уникальный идентификатор чата
        public string? Name { get; set; }  // Название чата (для группового чата)
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public ChatType? Type { get; set; }  // Тип чата (личный, групповой)
        
        public ICollection<UserChat> UserChats { get; set; } // Участники чата
        public ICollection<Message> Messages { get; set; } // сообщения, имеющие отношение к конкретному чату
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