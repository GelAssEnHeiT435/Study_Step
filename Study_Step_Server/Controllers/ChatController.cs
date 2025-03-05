using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Services;

namespace Study_Step_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUoW _unitOfWork;
        private readonly DtoConverterService _dtoConverter;
        private readonly IImageService _imageService;

        public ChatController(IUoW unitOfWork, 
                              DtoConverterService converter,
                              IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _dtoConverter = converter;
            _imageService = imageService;
        }

        #region get all chats for user
        [HttpGet("getallchats")]
        public async Task<UserChatsResponse> GetChats([FromQuery] int userId)
        {
            Console.WriteLine("этап 1");
            IEnumerable<Chat> users_chats = await _unitOfWork.UserChats.GetChatsByUserIdAsync(userId);
            IEnumerable<ChatDTO> dtoChats = _dtoConverter.GetChatListDTO(users_chats);
            Console.WriteLine("этап 2");
            IEnumerable<UserChat?> userChatsByChatIds = await _unitOfWork.UserChats.GetLinksByChatIdsAsync(dtoChats);
            IEnumerable<UserChatDTO> ucDto = _dtoConverter.GetUserChatListDTO(userChatsByChatIds);

            foreach (var chat in dtoChats)
            {
                // check type is Individual
                if (chat.Type == ChatType.Individual)
                {
                    User? secondUser = await _unitOfWork.UserChats.FindSecondUserAsync(chat.ChatId, userId); // find second user in chat
                    if (secondUser != null)
                    {
                        chat.ContactPhoto = _imageService.ConvertImageToByteArray(secondUser.ContactPhoto);
                        chat.Name = secondUser.Username;  // Устанавливаем имя второго пользователя как название чата
                    }
                }
            }
            Console.WriteLine("этап 3");
            UserChatsResponse response = new UserChatsResponse()
            {
                Chats = dtoChats,
                UserChats = ucDto
            };

            return response;
        }
        #endregion

        #region get all messages
        [HttpGet("messages")]
        public async Task<IEnumerable<MessageDTO>> GetMessage([FromQuery] int chatId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesByChatIdAsync(chatId);
            return _dtoConverter.GetMessageListDTO(messages);
        }
        #endregion

        #region add new message
        [HttpPost("newmessage")]
        public async Task<ActionResult> PostMessage(MessageDTO messageDto)
        {
            Message message = _dtoConverter.GetMessage(messageDto);

            // Сохранение сообщения в базе данных
            await _unitOfWork.Messages.AddAsync(message);
            return Ok();
        }
        #endregion

        #region get all users
        [HttpGet("selectuser")]
        public async Task<IEnumerable<UserDTO>> SelectUser()
        {
            IEnumerable<User> users = await _unitOfWork.Users.GetAllAsync();
            return _dtoConverter.GetUserListDTO(users);
        }
        #endregion
    }
}
