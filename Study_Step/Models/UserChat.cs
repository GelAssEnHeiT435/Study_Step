namespace Study_Step.Models
{
    public class UserChat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }

        public User? User { get; set; }
        public Chat? Chat { get; set; }
    }
}
