using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Services;


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
        public async Task SendMessage(string receiver, MessageDTO message)
        {
            Message messageObject = _dtoConverter.GetMessage(message);
            await _unitOfWork.Messages.AddAsync(messageObject);

            if (Context.UserIdentifier is string userName) // получаем юзера из контекста и отправляем сообщение от его лица получателю
            {
                await Clients.Users(receiver).SendAsync("ReceiveMessage", userName, message);
            }
        }

        // Метод обработки подключений к хабу
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"Приветствуем {Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
    }
}
