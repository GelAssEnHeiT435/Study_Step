using System.ComponentModel.DataAnnotations.Schema;

namespace Study_Step_Server.Models.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }  // Уникальный идентификатор чата
        public string? Name { get; set; }  // Название чата (для группового чата)
        public byte[]? ContactPhoto { get; set; }
        public string? Message { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public ChatType Type { get; set; }  // Тип чата (личный, групповой)
    }
}
