namespace Study_Step_Server.Models
{
    public class MessageRead
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public DateTime ReadAt { get; set; }


        public Message Message { get; set; }
        public User User { get; set; }
    }
}
