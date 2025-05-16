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

        public event Action<UserDTO> AddNewUserEvent;
        public event Action<string, ChatDTO, MessageDTO> OnMessageReceivedEvent;
        public event Action<string, ChatDTO, MessageDTO> OnUpdateIdEvent;
        public event Action<ChatDTO, MessageDTO> DeletionMessageEvent;
        public event Action<ChatDTO, MessageDTO> EditableMessageEvent;
        public event Action<int> ReadingMessageEvent;

        public SignalRService(SynchronizationContext synchronizationContext)
        {
            _synchronizationContext = synchronizationContext;
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chathub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_token);
                })
                .Build();

            _connection.On<UserDTO>("ReceiveNewUser", (userDTO) => {
                _synchronizationContext.Post(_ => {
                    AddNewUserEvent?.Invoke(userDTO);
                }, null);
            });

            _connection.On<string, ChatDTO, MessageDTO>("ReceiveMessage", (sender, chat, message) => {
                _synchronizationContext.Post(_ => {
                    OnMessageReceivedEvent?.Invoke(sender, chat, message);
                }, null);
            });

            _connection.On<string, ChatDTO, MessageDTO>("UpdateId", (sender, chat, message) => {
                _synchronizationContext.Post(_ => {
                    OnUpdateIdEvent?.Invoke(sender, chat, message);
                }, null);
            });

            _connection.On<ChatDTO, MessageDTO>("DeletionMessage", (chatDTO, messageDTO) => {
                _synchronizationContext.Post(_ => {
                    DeletionMessageEvent?.Invoke(chatDTO, messageDTO);
                }, null);
            });

            _connection.On<ChatDTO, MessageDTO>("EditableMessage", (chatDTO, messageDTO) => {
                _synchronizationContext.Post(_ => {
                    EditableMessageEvent?.Invoke(chatDTO, messageDTO);
                }, null);
            });

            _connection.On<int>("ReadingMessage", (chatId) => {
                _synchronizationContext.Post(_ => {
                    ReadingMessageEvent?.Invoke(chatId);
                }, null);
            });
        }

        #endregion

        #region Logics
        private string _token;

        public async Task ConnectAsync(string token) // connection to hub
        {
            _token = token;
            await _connection.StartAsync();
        }

        public async Task NotifyNewUser(UserDTO userDTO) =>
            await _connection.InvokeAsync("CreateNewUser", userDTO);
        public async Task SendMessageAsync(string reciever, ChatDTO chat, MessageDTO message) => // Метод отправки сообщения пользователю через хаб
            await _connection.InvokeAsync("SendMessage", reciever, chat, message);

        public async Task DeleteMessageAsync(string receiver, ChatDTO chatDTO, MessageDTO messageDTO, bool IsDeletedForAll) =>
            await _connection.InvokeAsync("DeleteMessage", receiver, chatDTO, messageDTO, IsDeletedForAll);

        public async Task EditMessageAsync(string receiver, ChatDTO chatDTO, MessageDTO messageDTO) =>
            await _connection.InvokeAsync("EditMessage", receiver, chatDTO, messageDTO);

        public async Task ReadMessageAsync(string receiver, int chatId) =>
            await _connection.InvokeAsync("ReadChatMessage", receiver, chatId);

        public async Task DisconnectAsync() => // disconnection to hub
            await _connection.StopAsync();

        #endregion
    }
}
