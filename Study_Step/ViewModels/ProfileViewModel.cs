using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Commands;
using Study_Step.Models;
using Study_Step.Models.DTO;
using Study_Step.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Study_Step.ViewModels
{
    class ProfileViewModel
    {
        private readonly HttpClient client = new HttpClient();
        private readonly DtoConverterService _dtoConverter;
        private readonly UserSessionService _userSession;
        public ProfileViewModel(DtoConverterService dtoConverter, UserSessionService userSession)
        {
            _dtoConverter = dtoConverter;
            _userSession = userSession;
        }
        #region ProfileInfo

        #region Properties
       
        private User _statusThumbsUser;
        public User statusThumbsUser
        {
            get => _statusThumbsUser;
            set
            {
                _statusThumbsUser = value;
                OnPropertyChanged(nameof(statusThumbsUser));
            }
        }

        public event Action<Chat> OpenChatRequested;

        #endregion

        #region Commands
        private ICommand _createNewChat;
        public ICommand CreateNewChat => _createNewChat ??= new RelayCommand(async _ =>
        {
            int clientID = _userSession.CurrentUser.UserId;

            HttpResponseMessage response = await client.GetAsync($"http://localhost:5000/api/chat/check_chat?clientId={clientID}&companionId={statusThumbsUser.UserId}"); // Send request to server

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync(); // Read response
            var jsonResponse = JsonConvert.DeserializeObject<ChatDTO>(responseBody); // Deserialize JSON to collection
            Chat chat = _dtoConverter.GetChat(jsonResponse);
            chat.bitmapPhoto = statusThumbsUser.bitmapPhoto;
            chat.Name = statusThumbsUser.Username;
            chat.UserId_InChat = chat.UserChats.Where(x => x.UserId != _userSession.CurrentUser.UserId)
                                               .Select(x => x.UserId)   
                                               .FirstOrDefault();

            OpenChatRequested?.Invoke(chat);
        });
        #endregion

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
