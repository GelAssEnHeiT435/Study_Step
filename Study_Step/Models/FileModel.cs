using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Study_Step.Models
{
    public class FileModel : INotifyPropertyChanged
    {
        public int FileModelId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; } // For example .jpg
        public long Size { get; set; } // size (byte)
        public string MimeType { get; set; } 
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }


        public int MessageId { get; set; }
        public Message Message { get; set; }


        [JsonIgnore]
        public CancellationTokenSource? CancellationTokenSource { get; set; }
        [JsonIgnore]
        public double Progress
        {
            get => _progress;
            set {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }   
        [JsonIgnore]
        public SendingStatus Status
        {
            get => _status;
            set {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private double _progress;
        private SendingStatus _status;

        [JsonIgnore]
        public long BytesDownloaded { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public enum SendingStatus
    {
        Success,
        Sending,
        Cancelled,
        Failed
    }
}
