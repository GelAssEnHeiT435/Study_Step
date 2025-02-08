using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Services;

namespace Study_Step_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationContext _context;
        //private byte[] fileBytes;

        public ChatController(ApplicationContext context)
        {
            _context = context; ;
        }

        #region get all chats for user
        [HttpGet("user_chats")]
        public IEnumerable<Chat> GetChats([FromQuery] int userId)
        {
            // Получение всех чатов, в которых состоит пользователь с данным userId
            IEnumerable<Chat> userChats = (from uc in _context.User_Chats
                                           where uc.UserId == userId  // Фильтр по идентификатору пользователя
                                           orderby uc.Chat.LastMessageTime descending //сортировка в порядке убывания
                                           select uc.Chat).ToList();  // Извлекаем только Chat
            foreach (var chat in userChats)
            {
                // Проверяем, что тип чата индивидуальный
                if (chat.Type == ChatType.Individual)
                {
                    // Находим второго пользователя в чате
                    User? secondUser = (from uc in _context.User_Chats
                                        where uc.ChatId == chat.Id && uc.UserId != userId
                                        select uc.User).FirstOrDefault();  // Берем первого (единственного) второго пользователя

                    // Если второй пользователь найден, устанавливаем его имя как название чата
                    if (secondUser != null)
                    {
                        string? photoPath;
                        if (secondUser.ContactPhoto != null)
                            photoPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", secondUser.ContactPhoto);
                        else
                            photoPath = null;

                        // Проверяем, существует ли файл
                        if (System.IO.File.Exists(photoPath))  // Возвращаем ошибку, если файл не найден
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(photoPath); // Читаем файл в массив байтов

                            // Преобразуем массив байтов в строку base64 и сохраняем в ContactPhoto
                            chat.ContactPhoto = fileBytes;
                        }

                        chat.Name = secondUser.Username;  // Устанавливаем имя второго пользователя как название чата
                    }
                }
            }
            // Возвращаем список чатов
            return userChats;
        }
        #endregion

        #region get all messages
        [HttpGet("messeges")]
        public IEnumerable<Message> GetMessage([FromQuery] int chatId)
        {
            Console.WriteLine(chatId);
            // Получение всех сообщений, принадлежащих определенному чату
            IEnumerable<Message> messages = (from mes in _context.Messages
                                             where mes.ChatId == chatId  // Фильтруем по UserId
                                             orderby mes.SentAt // сортировка по дате
                                             select mes).ToList();
            return messages;
        }
        #endregion

        #region add new message
        [HttpPost("newmessage")]
        public async Task<ActionResult> PostMessage(MessageDTO messageDTO)
        {
            // Создание нового сообщения
            var message = new Message
            {
                SenderId = messageDTO.SenderId,
                ChatId = messageDTO.ChatId,
                Text = messageDTO.Text,
                SentAt = DateTime.UtcNow,
                Type = messageDTO.Type,
                FileUrl = messageDTO.FileUrl,
            };

            // Сохранение сообщения в базе данных
            _context.Entry(message).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region get all users
        [HttpGet("selectuser")]
        public IEnumerable<UserDTO> SelectUser()
        {
            IEnumerable<UserDTO> users = (from user in _context.Users
                                          select new UserDTO
                                          {
                                              Id = user.Id,
                                              Username = user.Username,
                                              ContactPhoto = user.GetContactPhotoAsBytes(),
                                              Email = user.Email,
                                              Status = user.Status,
                                              LastLogin = user.LastLogin,
                                          }).ToList();
            return users;
        }
        #endregion
    }
}
