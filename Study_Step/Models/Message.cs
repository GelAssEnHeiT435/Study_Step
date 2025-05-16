using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Study_Step.Models
{
    public class Message: INotifyPropertyChanged
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }  // Идентификатор отправителя
        public int ChatId { get; set; }  // Идентификатор чата
        public string? Text // Текст сообщения
        { 
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }  
        public DateTime SentAt { get; set; }  // Время отправки
        public ObservableCollection<FileModel>? Files
        {
            get => _files;
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }
        private ObservableCollection<FileModel> _files;

        public bool isRead 
        {
            get => _isRead;
            set
            {
                _isRead = value;
                OnPropertyChanged(nameof(isRead));
            }
        }
        private bool _isRead;

        // True - извне
        // False - наше
        public bool? IsOutside { get; set; }

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
        private string? _text;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
