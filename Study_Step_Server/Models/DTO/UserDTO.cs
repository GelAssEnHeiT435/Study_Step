namespace Study_Step_Server.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[]? ContactPhoto { get; set; }
        public string Email { get; set; }
        public UserStatus? Status { get; set; }  // Статус пользователя (онлайн, оффлайн)
        public DateTime? LastLogin { get; set; }  // Время последнего входа
    }
}
