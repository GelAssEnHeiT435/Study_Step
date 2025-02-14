using Microsoft.AspNetCore.SignalR.Client;
using Study_Step.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Study_Step
{
    public class SignalRService
    {
        #region Events&Connections
        private HubConnection _connection;
        private readonly SynchronizationContext _synchronizationContext;

        public event Action<string, MessageDTO> OnMessageReceived;

        public SignalRService(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chathub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token);
                })
                .Build();

            _connection.On<string, MessageDTO>("ReceiveMessage", (sender, message) =>
            {
                // Используем SynchronizationContext для вызова в UI-потоке
                _synchronizationContext.Post(_ => {
                    OnMessageReceived?.Invoke(sender, message); // Вызов события в UI-потоке
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


        public async Task SendMessageAsync(string reciever, MessageDTO message) => // Метод отправки сообщения пользователю через хаб
            await _connection.InvokeAsync("SendMessage", reciever, message);

        #endregion
    }
}
