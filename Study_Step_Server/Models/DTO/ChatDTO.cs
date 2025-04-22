using System.ComponentModel.DataAnnotations.Schema;

namespace Study_Step_Server.Models.DTO
{
    public class ChatDTO
    {
        public int ChatId { get; set; }  // Уникальный идентификатор чата
        public string? Name { get; set; }  // Название чата (для группового чата)
        public byte[]? ContactPhoto { get; set; }
        public string? LastMessage { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public ChatType Type { get; set; }  // Тип чата (личный, групповой)

        public List<UserChatDTO> UserChats { get; set; } // Участники чата
    }
}
