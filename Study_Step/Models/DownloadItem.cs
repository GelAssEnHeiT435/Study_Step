using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Study_Step.Models
{
    public class DownloadItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public string SavePath { get; set; }
        [JsonIgnore]
        public string tempPath { get; } = $"download_{Guid.NewGuid()}.tmp";
        public DateTime DownloadTime { get; set; }

        public DownloadStatus Status
        {
            get => _status;
            set 
            { 
                _status = value; 
                OnPropertyChanged(nameof(Status));
            }
        }
        [JsonIgnore]
        public double Progress
        {
            get => _progress;
            set 
            { 
                _progress = value; 
                OnPropertyChanged(nameof(Progress)); 
            }
        }
        private double _progress;
        private DownloadStatus _status;

        [JsonIgnore]
        public CancellationTokenSource CancellationTokenSource { get; set; }

        [JsonIgnore]
        public long BytesDownloaded { get; set; }

        public string GetTempFile() {
            string? path = ((App)Application.Current).Configuration["AppSettings:SavePath"];
            return Path.Combine(path, tempPath);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public enum DownloadStatus
    {
        Downloading,
        Paused,
        Completed,
        Failed,
        Cancelled
    }
}
