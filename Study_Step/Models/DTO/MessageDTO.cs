namespace Study_Step.Models.DTO
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
        public List<FileModelDTO>? Files { get; set; }
        public bool isRead { get; set; }
    }
}
