namespace Study_Step_Server.Models.DTO
{
    public class DeletedMessageDTO
    {
        public int DeletedMessageId { get; set; }

        public int UserId { get; set; }
        public UserDTO User { get; set; }

        public int ChatId { get; set; }
        public ChatDTO Chat { get; set; }

        public int MessageId { get; set; }
        public MessageDTO Message { get; set; }

        public DateTime DeletedAt { get; set; }
        public bool IsDeletedForEveryone { get; set; }
    }
}
