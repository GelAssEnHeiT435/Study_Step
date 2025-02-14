using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;


namespace Study_Step_Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _context;
        public ChatHub(ApplicationContext context) { _context = context; }

        [Authorize]
        public async Task SendMessage(string receiver, MessageDTO message)
        {
            // TODO: сохранение сообщения в базу данных (Events or dbContext?)
            Message messageObject = new Message()
            {
                SenderId = message.SenderId,
                ChatId = message.ChatId,
                Text = message.Text,
                SentAt = message.SentAt,
                Type = message.Type,
                FileUrl = message.FileUrl
            };
            _context.Messages.Add(messageObject);
            await _context.SaveChangesAsync();

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
