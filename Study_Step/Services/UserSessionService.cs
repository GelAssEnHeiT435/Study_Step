using Study_Step.Models;
using Study_Step.Models.DTO;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Study_Step.Services
{
    public class UserSessionService : INotifyPropertyChanged
    {
        private readonly DtoConverterService _dtoConverter;
        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        private User? _currentUser;

        public UserSessionService(DtoConverterService dtoConverter)
        {
            _dtoConverter = dtoConverter;
        }

        public UserDTO GetCurrentUserDTO()
        {
            return _dtoConverter.GetUserDTO(_currentUser);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
