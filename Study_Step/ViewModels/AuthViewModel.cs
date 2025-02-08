using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Study_Step.ViewModels
{
    class AuthViewModel: INotifyPropertyChanged
    {
        #region Properties
        private string? _username;
        public string? Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    System.Diagnostics.Debug.WriteLine(value);
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        private string? _email;
        public string? Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    System.Diagnostics.Debug.WriteLine(value);
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    System.Diagnostics.Debug.WriteLine(value);
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        #endregion
        public AuthViewModel() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
