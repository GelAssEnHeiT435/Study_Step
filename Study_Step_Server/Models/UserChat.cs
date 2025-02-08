namespace Study_Step_Server.Models
{
    public class UserChat
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }

        public virtual User? User { get; set; }
        public virtual Chat? Chat { get; set; }
    }
}
