using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Study_Step.Models
{
    public class Chat : INotifyPropertyChanged
    {
        public int ChatId { get; set; }  // Уникальный идентификатор чата
        public string? Name { get; set; }  // Название чата (для группового чата)
        public BitmapImage? bitmapPhoto { get; set; }
        public string? LastMessage
        {
            get => _lastMessage;
            set 
            {
                _lastMessage = value;
                OnPropertyChanged(nameof(LastMessage));
            }
        }
        public DateTime LastMessageTime { get; set; }
        public string TimeAgo
        {
            get => _timeAgo;
            set
            {
                _timeAgo = value;
                OnPropertyChanged(nameof(TimeAgo));
            }
        }
        private string _timeAgo;
        private string? _lastMessage;

        public int UnreadCount
        {
            get => _unreadCount;
            set
            {
                _unreadCount = value;
                OnPropertyChanged(nameof(UnreadCount));
            }
        }
        private int _unreadCount = 0;

        public bool IsChoosen
        {
            get => _isChoosen;
            set
            {
                _isChoosen = value;
                OnPropertyChanged(nameof(IsChoosen));
            }
        }
        private bool _isChoosen;

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }
        private bool _isPopupOpen = false;


        public int? UserId_InChat { get; set; }
        public List<UserChat> UserChats { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}