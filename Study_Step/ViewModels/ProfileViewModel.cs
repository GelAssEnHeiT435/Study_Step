using Newtonsoft.Json.Linq;
using Study_Step.Commands;
using Study_Step.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Study_Step.ViewModels
{
    class ProfileViewModel
    {
        #region ProfileInfo

        #region Properties
        private User _statusThumbsUser;
        public User statusThumbsUser
        {
            get => _statusThumbsUser;
            set
            {
                _statusThumbsUser = value;
                OnPropertyChanged(nameof(statusThumbsUser)); // Оповещаем об изменении
            }
        }
        #endregion

        #region Commands
        private ICommand _createNewChat;
        public ICommand CreateNewChat => _createNewChat ??= new RelayCommand(parameter =>
        {
            ViewModel viewModel;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == "ChatWindow")
                {
                    viewModel = (ViewModel)window.DataContext;
                    // Отображение заголовка чата с передачей данных
                    viewModel.ChatIsActive = true;

                    viewModel.ContactName = statusThumbsUser.Username;
                    viewModel.bitmapPhoto = statusThumbsUser.Photo;

                    // загрузка сообщений конкретного чата
                    //Application.Current.Properties["ChatId"] = statusThumbsUser.Id;
                }
            }
            
            
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
