namespace Study_Step_Server.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime Created { get; set; }


        public int UserId { get; set; }
        public AuthUser User { get; set; }
    }
}
