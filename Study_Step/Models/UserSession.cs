namespace Study_Step.Models
{
    public class UserSession
    {
        public int UserId { get; set; }  // Идентификатор пользователя
        public string Token { get; set; }  // JWT токен для авторизации
        public DateTime Expiration { get; set; }  // Время истечения срока действия токена
    }
}
