namespace Study_Step_Server.Models
{
    public class UserChat
    {
        public int UserChatId { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }

        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
