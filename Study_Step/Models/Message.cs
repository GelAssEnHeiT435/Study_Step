namespace Study_Step.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }  // Идентификатор отправителя
        public int ChatId { get; set; }  // Идентификатор чата
        public string? Text { get; set; }  // Текст сообщения
        public DateTime SentAt { get; set; }  // Время отправки
        public List<FileModel>? Files { get; set; }

        // True - извне
        // False - наше
        public bool? IsOutside { get; set; }
        //public User? Sender { get; set; }
        //public Chat? Chat { get; set; }
    }
}
