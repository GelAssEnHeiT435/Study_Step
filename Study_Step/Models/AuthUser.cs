namespace Study_Step.Models
{
    public class AuthUser
    {
        public int Id { get; set; } 
        public string? Name { get; set; } // Имя пользователя
        public string Email { get; set; } // Почта пользователя
        public string Password { get; set; } // Хэшированное значение пароля в БД

    }
}
