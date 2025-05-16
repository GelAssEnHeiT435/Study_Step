using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Study_Step_Server.Models
{
    public class DeletedMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeletedMessageId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ChatId { get; set; }
        public Chat Chat { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }

        public DateTime DeletedAt { get; set; }
        public bool IsDeletedForEveryone { get; set; }
    }
}
