using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Study_Step_Server.Models
{
    public class UserChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserChatId { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public DateTime? LastReadTime { get; set; }

        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
