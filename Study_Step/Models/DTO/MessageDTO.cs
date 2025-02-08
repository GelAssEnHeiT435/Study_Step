namespace Study_Step.Models.DTO
{
    public class MessageDTO
    {
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public string? Text { get; set; }
        public MessageType Type { get; set; }
        public string? FileUrl { get; set; }
    }
}
