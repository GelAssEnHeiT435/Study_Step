namespace Study_Step.Models.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public byte[]? ContactPhoto { get; set; }
        public string Email { get; set; }
        public UserStatus? Status { get; set; }  // Статус пользователя (онлайн, оффлайн)
        public DateTime? LastLogin { get; set; }  // Время последнего входа
    }
}
