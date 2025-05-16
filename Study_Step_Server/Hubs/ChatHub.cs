using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Services;
using System.Security.Claims;


namespace Study_Step_Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUoW _unitOfWork;
        private readonly DtoConverterService _dtoConverter;
        public ChatHub(IUoW unitOfWork, DtoConverterService converter) 
        {
            _unitOfWork = unitOfWork;
            _dtoConverter = converter;
        }

        [Authorize]
        public async Task CreateNewUser(UserDTO userDTO) =>
            await Clients.Others.SendAsync("ReceiveNewUser", userDTO);

        [Authorize]
        public async Task SendMessage(string receiver, ChatDTO chat, MessageDTO message)
        {
            Message messageObject = _dtoConverter.GetMessage(message);
            Chat chatObject = _dtoConverter.GetChat(chat);
            chatObject.Name = null;

            await _unitOfWork.Messages.AddAsync(messageObject);
            await _unitOfWork.Chats.UpdateAsync(chatObject);
            await _unitOfWork.DeletedChats.RestoreChatAsync(int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value), chat.ChatId);

            chat.ChatId = chatObject.ChatId;
            message.MessageId = messageObject.MessageId;
            message.Files = _dtoConverter.GetFileListDTO(messageObject.Files).ToList();

            if (Context.UserIdentifier is string userName)
            {
                await Clients.Caller.SendAsync("UpdateId", userName, chat, message);
                await Clients.Users(receiver).SendAsync("ReceiveMessage", userName, chat, message);
            }
        }

        [Authorize]
        public async Task DeleteMessage(string receiver, ChatDTO chatDTO, MessageDTO messageDTO, bool IsDeletedParam)
        {
            DeletedMessage dMessage = new DeletedMessage()
            {
                UserId = int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                ChatId = chatDTO.ChatId,
                MessageId = messageDTO.MessageId,
                DeletedAt = DateTime.UtcNow,
                IsDeletedForEveryone = IsDeletedParam
            };
            await _unitOfWork.DeletedMessages.AddAsync(dMessage);

            bool shouldDeleteForAll = !await _unitOfWork.DeletedMessages
                                                        .IsMessageDeletedForOtherUser(int.Parse(receiver),
                                                                                      messageDTO.MessageId);
            if (IsDeletedParam && shouldDeleteForAll)
            {
                Chat chatObject = _dtoConverter.GetChat(chatDTO);
                chatObject.Name = null;
                await _unitOfWork.Chats.UpdateAsync(chatObject);

                await Clients.Users(receiver).SendAsync("DeletionMessage", chatDTO, messageDTO);
            }   
        }

        [Authorize]
        public async Task EditMessage(string receiver, ChatDTO chatDTO, MessageDTO messageDTO)
        {
            Message messageObject = _dtoConverter.GetMessage(messageDTO);
            Chat chatObject = _dtoConverter.GetChat(chatDTO);
            chatObject.Name = null;

            await _unitOfWork.Messages.UpdateMessageWithFiles(messageObject);
            await _unitOfWork.Chats.UpdateAsync(chatObject);

            await Clients.Users(receiver).SendAsync("EditableMessage", chatDTO, messageDTO);
        }

        [Authorize]
        public async Task ReadChatMessage(string receiver, int chatId)
        {
            await _unitOfWork.Messages.MarkAllAsRead(chatId,
                int.Parse(Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value));

            await Clients.Users(receiver).SendAsync("ReadingMessage", chatId);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
    }
}
