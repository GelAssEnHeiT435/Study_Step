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
        private readonly IFileService _fileService;

        public ChatController(IUoW unitOfWork, 
                              DtoConverterService converter,
                              IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _dtoConverter = converter;
            _fileService = fileService;
        }

        #region get all chats for user
        [HttpGet("getallchats")]
        public async Task<List<ChatDTO>> GetChats([FromQuery] int userId)
        {
            List<Chat> users_chats = await _unitOfWork.UserChats.GetChatsByUserIdAsync(userId);
            List<UserChat> userChatsByChatIds = await _unitOfWork.UserChats.GetLinksByChatIdsAsync(users_chats);
            List<ChatDTO> dtoChats = _dtoConverter.GetChatListDTO(users_chats).ToList();
            IEnumerable<UserChatDTO> ucDto = _dtoConverter.GetUserChatListDTO(userChatsByChatIds);

            foreach(var chat in dtoChats) 
            {
                if (chat.Type == ChatType.Individual)
                {
                    User? secondUser = await _unitOfWork.UserChats.FindSecondUserAsync(chat.ChatId, userId); // find second user in chat
                    if (secondUser != null)
                    {
                        chat.ContactPhoto = _fileService.ConvertFileToByteArray(secondUser.ContactPhoto);
                        chat.Name = secondUser.Username;  // Устанавливаем имя второго пользователя как название чата
                    }
                } 
                chat.UserChats = ucDto.Where(c => c.ChatId == chat.ChatId).ToList();
            }

            return dtoChats;
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

        [HttpGet("check_chat")]
        public async Task<ActionResult<ChatDTO?>> CheckChatExists([FromQuery] int clientId,
                                                                  [FromQuery] int companionId)
        {
            Chat? chat = await _unitOfWork.Chats.SearchIndividualChatAsync(clientId, companionId);
            if (chat != null) return Ok(_dtoConverter.GetChatDTO(chat));

            Chat newChat = new Chat()
            {
                Type = ChatType.Individual,
                UserChats = new List<UserChat>
                {
                    new UserChat { UserId = clientId },
                    new UserChat { UserId = companionId }
                }
            };
            await _unitOfWork.Chats.AddAsync(newChat);
            return Ok(_dtoConverter.GetChatDTO(newChat));
        }
    }
}
