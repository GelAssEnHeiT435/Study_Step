using Microsoft.AspNetCore.Mvc;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
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

            foreach (var chat in dtoChats)
            {
                // Получаем последнее неудалённое сообщение для этого пользователя
                var lastValidMessage = await _unitOfWork.Messages.GetLastUndeletedMessageAsync(chat.ChatId, userId);

                if (lastValidMessage != null)
                {
                    if (lastValidMessage.Text == null && lastValidMessage.Files.Any())
                        chat.LastMessage = "файл";
                    else
                        chat.LastMessage = lastValidMessage.Text;
                    chat.LastMessageTime = lastValidMessage.SentAt;
                }

                if (chat.Type == ChatType.Individual)
                {
                    User? secondUser = await _unitOfWork.UserChats.FindSecondUserAsync(chat.ChatId, userId);
                    if (secondUser != null)
                    {
                        chat.ContactPhoto = _fileService.ConvertFileToByteArray(secondUser.ContactPhoto);
                        chat.Name = secondUser.Username;
                    }
                }
                chat.UserChats = ucDto.Where(c => c.ChatId == chat.ChatId).ToList();
                chat.UnreadCount = await _unitOfWork.Messages.GetUnreadMessagesCount(chat.ChatId, userId);
            }

            return dtoChats.OrderByDescending(c => c.LastMessageTime).ToList();
        }
        #endregion

        #region get all messages
        [HttpGet("messages")]
        public async Task<IEnumerable<MessageDTO>> GetMessage([FromQuery] int userId,
                                                              [FromQuery] int chatId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesByChatIdAsync(userId, chatId);
            IEnumerable<MessageDTO> mesList = _dtoConverter.GetMessageListDTO(messages);

            foreach (var message in mesList) 
                message.isRead = await _unitOfWork.Messages.isReadMessageByIdAsync(message.MessageId);
                
            return mesList;
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

        [HttpDelete("deletechat")]
        public async Task DeleteChat([FromQuery] int userId,
                                     [FromQuery] int chatId)
        {
            await _unitOfWork.DeletedChats.DeleteChatForUserAsync(userId, chatId);
            await _unitOfWork.DeletedMessages.DeleteChatsMessagesAsync(userId, chatId);
        }
    }
}
