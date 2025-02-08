using Study_Step.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_Step.ViewModels
{
    class ProfileViewModel
    {
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
