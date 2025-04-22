using Microsoft.AspNetCore.SignalR.Client;
using Study_Step.Models.DTO;
using System.Net.Http;

namespace Study_Step.Services
{
    public class SignalRService
    {
        #region Events&Connections
        private HubConnection _connection;
        private readonly SynchronizationContext _synchronizationContext;

        public event Action<string, ChatDTO, MessageDTO> OnMessageReceived;

        public SignalRService(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chathub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token);
                })
                .Build();

            _connection.On<string, ChatDTO, MessageDTO>("ReceiveMessage", (sender, chat, message) =>
            {
                _synchronizationContext.Post(_ =>
                {
                    OnMessageReceived?.Invoke(sender, chat, message);
                }, null);
            });
        }
        #endregion

        #region Logics
        private string _token;

        public async Task ConnectAsync(string token) // Метод подключения к хабу
        {
            _token = token;
            await _connection.StartAsync();
        }

        public async Task DisconnectAsync() => // Метод отключения от хаба
            await _connection.StopAsync();


        public async Task SendMessageAsync(string reciever, ChatDTO chat, MessageDTO message) => // Метод отправки сообщения пользователю через хаб
            await _connection.InvokeAsync("SendMessage", reciever, chat, message);

        #endregion
    }
}
