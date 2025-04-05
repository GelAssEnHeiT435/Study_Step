using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Study_Step.Models
{
    public class Message: INotifyPropertyChanged
    {
        public int MessageId { get; set; }
        public int UserId { get; set; }  // Идентификатор отправителя
        public int ChatId { get; set; }  // Идентификатор чата
        public string? Text { get; set; }  // Текст сообщения
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

        // True - извне
        // False - наше
        public bool? IsOutside { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
