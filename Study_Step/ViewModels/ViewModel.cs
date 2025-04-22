using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Commands;
using Study_Step.CustomControls;
using Study_Step.Interfaces;
using Study_Step.Models;
using Study_Step.Models.DTO;
using Study_Step.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Study_Step.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        public static readonly HttpClient client = new HttpClient();
        private static readonly DispatcherTimer _timer = new();
        public static string apiUrl = "http://localhost:5000/api/chat";

        private readonly SignalRService _signalRService;
        private readonly IFileService _fileService;
        private readonly DtoConverterService _dtoConverter;
        private readonly AuthService _authService;
        private readonly UserSessionService _userSession;

        public ViewModel(SignalRService signalRService,
                         IFileService fileService,
                         DtoConverterService dtoConverter,
                         AuthService authService,
                         UserSessionService userSession)
        {
            _userSession = userSession;
            _signalRService = signalRService;
            _fileService = fileService;
            _dtoConverter = dtoConverter;
            _authService = authService;

            // Add EventsListeners
            _signalRService.OnMessageReceived += MessageReceived;

            LoadChats();
            LoadUserList();
            LoadDownloadedArea();

            ChatIsActive = false;
            DownloadAreaIsActive = false;

            _timer.Interval = TimeSpan.FromSeconds(60);
            _timer.Tick += (s, e) => UpdateAllChats();
            _timer.Start();
        }

        // General settings of application
        #region MainWindow

        #region Events

        public event Action ScrollToEnd;

        #endregion

        #region Properties

        public string ContactName
        {
            get => _contactName;
            set
            {
                _contactName = value;
                OnPropertyChanged(nameof(ContactName));
            }
        }
        public BitmapImage bitmapPhoto
        {
            get => _bitmapPhoto;
            set
            {
                _bitmapPhoto = value;
                OnPropertyChanged(nameof(bitmapPhoto));
            }
        }
        public string LastSeen
        {
            get => _lastSeen;
            set
            {
                _lastSeen = value;
                OnPropertyChanged(nameof(LastSeen));
            }
        }
        public bool ChatIsActive
        {
            get => _ChatIsActive;
            set
            {
                _ChatIsActive = value;
                OnPropertyChanged(nameof(ChatIsActive));
            }
        }

        private string _contactName;
        private BitmapImage _bitmapPhoto;
        private string _lastSeen;
        private bool _ChatIsActive;

        #endregion

        #endregion

        // Show chat list
        #region Chat List

        #region Properties

        public ObservableCollection<Chat> Chats { get; set; }
        public Chat? CurrentChat { get; set; }

        #endregion

        #region Logics
        private async void LoadChats()
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/getallchats?userId={_userSession.CurrentUser.UserId}"); // Send Request to Server

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync(); // Read body response
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<ChatDTO>>(responseBody); // Deserialize object to collection

                Chats = new ObservableCollection<Chat>();

                foreach (var chat in jsonResponse)
                {
                    Chat thisChat = _dtoConverter.GetChat(chat); // convert ChatDTO to Chat
                    foreach (var uc in thisChat.UserChats) // Get User's Id to send messages
                    {
                        if (uc.UserId != _userSession.CurrentUser.UserId && chat.ChatId == uc.ChatId) {
                            thisChat.UserId_InChat = uc.UserId;
                        }
                    }
                    UpdateMessage(thisChat);
                    Chats.Add(thisChat);
                }
            }
            else
            {
                Console.WriteLine("Ошибка при получении чатов");
            }
            OnPropertyChanged(nameof(Chats));
        }

        private void UpdateChatList(Chat updatedChat)
        {
            Chat? removeChat = Chats.FirstOrDefault(chat => chat.ChatId == updatedChat.ChatId);
            if (updatedChat != null
                && Chats.Any(chat => chat.ChatId == updatedChat.ChatId)) // If chat not exists, then add to start list
                Chats.Remove(removeChat);
            Chats.Insert(0, updatedChat);
        }
        #endregion

        #region Commands
        private ICommand _getSelectedChatCommand;
        public ICommand GetSelectedChatCommand => _getSelectedChatCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is Chat chat) // получение имени и фото пользователя при выборе чата
            {
                // Отображение заголовка чата с передачей данных
                ChatIsActive = true;
                CurrentChat = chat;
                CurrentChat.UnreadCount = 0;
                ContactName = CurrentChat.Name;
                bitmapPhoto = CurrentChat.bitmapPhoto;
                FilesListObject = new ObservableCollection<FileModel>();

                LoadChatConversation(chat.ChatId);
            }
        });
        #endregion

        #region Timer

        private void UpdateAllChats() {
            foreach (var chat in Chats)
                UpdateMessage(chat);
        }

        private void UpdateMessage(Chat chat)
        {
            DateTime utcTime = chat.LastMessageTime;
            DateTime localTime = utcTime.ToLocalTime();
            chat.TimeAgo = FormatTimeAgo(localTime);
        }

        private string FormatTimeAgo(DateTime time)
        {
            var span = DateTime.Now - time;
            if (span.TotalSeconds < 60) return "только что";
            if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} {Pluralize(span.Minutes, "минуту", "минуты", "минут")} назад";
            if (span.TotalHours < 24) return $"{(int)span.TotalHours} {Pluralize(span.Hours, "час", "часа", "часов")} назад";
            if (time.Date == DateTime.Today.AddDays(-1)) return $"вчера в {time:HH:mm}";
            return $"{time:dd.MM.yyyy} в {time:HH:mm}";
        }
        private string Pluralize(int number, string form1, string form2, string form5)
        {
            number = Math.Abs(number);
            int n1 = number % 10;
            int n2 = number % 100;

            return (n1 == 1 && n2 != 11) ? form1 :
                   (n1 >= 2 && n1 <= 4 && (n2 < 10 || n2 >= 20)) ? form2 : form5;
        }

        #endregion

        #endregion

        // Functions for chats
        #region ChatConversation

        #region Properties

        public ObservableCollection<Message>? Conversations { 
            get => mConversations;
            set {
                mConversations = value;
                OnPropertyChanged(nameof(Conversations));
            }}
        public ObservableCollection<FileModel> FilesListObject {
            get => _filesList;
            set {
                _filesList = value;
                OnPropertyChanged(nameof(FilesListObject));
            }}
        public string? MessageText {
            get => _messageText;
            set {
                _messageText = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged(nameof(MessageText));
            }}

        private ObservableCollection<Message> mConversations;
        private ObservableCollection<FileModel> _filesList;
        private string? _messageText { get; set; }

        #endregion

        #region Logics
        private async void LoadChatConversation(int Id)
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/messages?chatId={Id}"); // Send request to server

            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync(); // Read response
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<MessageDTO>>(responseBody); // Deserialize JSON to collection
                IEnumerable<Message> messages = _dtoConverter.GetMessageList(jsonResponse);

                Conversations = new ObservableCollection<Message>();

                // Отображаем полученные чаты
                foreach (var mes in messages)
                {
                    mes.IsOutside = mes.UserId != _userSession.CurrentUser.UserId;
                    Conversations.Add(mes);
                    ScrollToEnd?.Invoke();
                }
            }
            else
            {
                Console.WriteLine("Ошибка при получении чатов");
            }
        }
        private void MessageReceived(string sender, ChatDTO chat, MessageDTO message)
        {
            Message messageObject = _dtoConverter.GetMessage(message);
            Chat chatObject = _dtoConverter.GetChat(chat);

            UpdateChatList(chatObject);

            Chat recieverChat = Chats.FirstOrDefault(x => x.ChatId == chatObject.ChatId);

            recieverChat.LastMessage = chatObject.LastMessage;
            recieverChat.LastMessageTime = chatObject.LastMessageTime;
            UpdateMessage(recieverChat);
            
            if (CurrentChat != null && CurrentChat.ChatId == chatObject.ChatId) // if a chat is open with the person who sent the message
            {
                messageObject.IsOutside = true; // changing the sender's side
                Conversations.Add(messageObject);
                ScrollToEnd?.Invoke();
            }
            else if (CurrentChat == null || CurrentChat.ChatId != chatObject.ChatId) // If a chat with another person is open or the chat is not open
            {
                recieverChat.UnreadCount++;
            }
            else // If the chats are not open
            {
                // TODO: add notify system
            }
        }

        #endregion

        #region Commands
        public ICommand SendMesCommand => _sendMesCommand ??= new RelayCommand(async _ =>
        {
            UpdateChatList(CurrentChat);
            Message messageChat = new Message()
            {
                UserId = _userSession.CurrentUser.UserId,
                ChatId = CurrentChat.ChatId,
                Text = string.IsNullOrWhiteSpace(MessageText) ? null : MessageText,
                SentAt = DateTime.UtcNow,
                Files = new ObservableCollection<FileModel>(FilesListObject.ToList()),
                IsOutside = false
            };
            Conversations.Add(messageChat);

            ScrollToEnd?.Invoke();
            MessageText = string.Empty;

            // add file to response
            foreach (FileModel file in messageChat.Files.ToList()) {
                try {
                    var buffer = new byte[81920];
                    file.CancellationTokenSource = new CancellationTokenSource();

                    await using var fileStream = File.OpenRead(file.Path);
                    var content = new ProgressableStreamContent(fileStream, 81920, file.CancellationTokenSource.Token)
                    {
                        Progress = (sentBytes, totalBytes) =>
                        {
                            file.Progress = (int)((double)sentBytes / totalBytes * 100);
                        }
                    };

                    var request = new HttpRequestMessage(HttpMethod.Post,
                                                         $"http://localhost:5000/api/fileupload/upload")
                    {
                        Content = content
                    };
                    request.Headers.Add("X-FileName", WebUtility.UrlEncode(file.Name));
                    request.Headers.Add("X-FileSize", file.Size.ToString());

                    var response = await client.SendAsync(request,
                                                          HttpCompletionOption.ResponseHeadersRead,
                                                          file.CancellationTokenSource.Token);
                    response.EnsureSuccessStatusCode();

                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeAnonymousType(responseJson, new { Path = "", Size = 0L });
                    file.Path = result.Path;
                    file.Status = SendingStatus.Success;
                }
                catch (OperationCanceledException) {
                    file.Status = SendingStatus.Cancelled;
                    Debug.WriteLine($"Отправка файла {file.Name} отменена");
                }
                catch (Exception ex) {
                    messageChat.Files.Remove(file);
                    Debug.WriteLine(ex);
                }
                finally {
                    file.CancellationTokenSource?.Dispose();
                    file.CancellationTokenSource = null;

                    FilesListObject.Remove(file);
                    if (string.IsNullOrWhiteSpace(messageChat.Text) & !messageChat.Files.Any()) {
                        Conversations.Remove(messageChat);
                    }   
                }
            }

            try
            {
                MessageDTO messageObject = _dtoConverter.GetMessageDTO(messageChat);

                if (messageChat.Text == null && messageChat.Files.Any())
                    CurrentChat.LastMessage = "файл";
                else
                    CurrentChat.LastMessage = messageChat.Text;
                
                CurrentChat.LastMessageTime = messageChat.SentAt;
                ChatDTO chatObject = _dtoConverter.GetChatDTO(CurrentChat);

                var userDto = _userSession.GetCurrentUserDTO();
                chatObject.ContactPhoto = userDto.ContactPhoto;
                chatObject.Name = userDto.Username;

                Debug.WriteLine($"chatObject: {JsonConvert.SerializeObject(chatObject)}");
                Debug.WriteLine($"messageObject: {JsonConvert.SerializeObject(messageObject)}");

                await _signalRService.SendMessageAsync(CurrentChat.UserId_InChat.ToString(), chatObject, messageObject);

                UpdateMessage(CurrentChat);
            }
            catch (HubException hex)
            {
                Debug.WriteLine($"{hex.Message}");
            }
        });
        public ICommand ChooseFile => _chooseFile ??= new RelayCommand(parameter =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true, // Включаем множественный выбор
                Filter = "All files (*.*)|*.*" // Фильтр файлов
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filePath in openFileDialog.FileNames) // Используем FileNames вместо FileName
                {
                    var file = new FileInfo(filePath);

                    FileModel newFile = new FileModel()
                    {
                        Name = file.Name,
                        Extension = file.Extension,
                        Size = file.Length,
                        MimeType = MimeMapping.MimeUtility.GetMimeMapping(filePath),
                        Path = filePath,
                        CreatedAt = file.CreationTime.ToUniversalTime(),
                        Status = SendingStatus.Sending
                    };

                    FilesListObject.Add(newFile);
                }
                FilesListObject = new ObservableCollection<FileModel>(FilesListObject.Take(5));
            }
        });

        public ICommand DeleteFile => _deleteFile ??= new RelayCommand(parameter =>
        {
            if (parameter is FileModel file) {
                if (FilesListObject.Contains(file)) { FilesListObject.Remove(file); }
            }
        });

        public ICommand DownloadFile => _downloadFile ??= new RelayCommand(async parameter =>
        {
            DownloadItem item;
            if (parameter is FileModel file) // Situation starting download
            {
                if (file.Status == SendingStatus.Sending) return;

                item = _dtoConverter.GetDownloadItem(file);
                DownloadAreaIsActive = !DownloadAreaIsActive; // Open popup
                ActiveDownloads.Add(item);
            }
            else if (parameter is DownloadItem fileItem) // sutuation resume download
            {
                item = fileItem;
            }
            else return;

            await _parallelLimiter.WaitAsync();
            try
            {
                if (item.Status == DownloadStatus.Cancelled)
                {
                    throw new OperationCanceledException("The download has already been stopped");
                }
                item.Status = DownloadStatus.Downloading;
                using (item.CancellationTokenSource = new CancellationTokenSource())
                {
                    string? savePath = ((App)Application.Current).Configuration["AppSettings:SavePath"];
                    string tempPath = item.GetTempFile();
                    if (File.Exists(item.tempPath))
                    {
                        item.BytesDownloaded = new FileInfo(tempPath).Length;
                    }
                        
                    string json = JsonConvert.SerializeObject(item.Id); // Convert Id to JSON-Object
                    var content = new StringContent(json, Encoding.UTF8, "application/json"); // Create HTTP-Request
                    var request = new HttpRequestMessage( HttpMethod.Get,
                                                          $"http://localhost:5000/api/fileupload/download?Id={item.Id}");
                    if (item.BytesDownloaded > 0)
                    {
                        request.Headers.Range = new RangeHeaderValue(item.BytesDownloaded, null);
                    }

                    item.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                    HttpResponseMessage response = await client.SendAsync(request,
                                                                          HttpCompletionOption.ResponseHeadersRead,
                                                                          item.CancellationTokenSource.Token); // Send request to server
                    response.EnsureSuccessStatusCode();

                    string? fileName = response.Content.Headers.ContentDisposition?.FileName;
                    if (fileName != null)
                    {
                        fileName = Uri.UnescapeDataString(fileName);
                    }
                    string path = Path.Combine(savePath, fileName);
                    item.SavePath = path;

                    await using (var fileStream = new FileStream(tempPath, FileMode.OpenOrCreate, 
                                                                 FileAccess.Write, FileShare.Read,
                                                                 8192, FileOptions.Asynchronous))
                    await using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        fileStream.Seek(item.BytesDownloaded, SeekOrigin.Begin);
                        var buffer = new byte[8192];
                        int bytesRead;

                        while (true)
                        {
                            item.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, item.CancellationTokenSource.Token);
                            if (bytesRead == 0) break; 

                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            item.BytesDownloaded += bytesRead;

                            if (item.BytesDownloaded > 0)
                                item.Progress = (item.BytesDownloaded * 100d) / item.Size;
                        }
                    }

                    Debug.WriteLine(tempPath, item.SavePath);
                    File.Move(tempPath, item.SavePath, overwrite: true);
                        
                    item.Status = DownloadStatus.Completed;
                    item.DownloadTime = DateTime.Now;
                    SaveToHistory(item);
                }
            }
            catch (OperationCanceledException)
            {
                item.Status = DownloadStatus.Cancelled;
                try { File.Delete(item.GetTempFile()); } catch { }
            }
            catch (Exception)
            {
                item.Status = DownloadStatus.Failed;
                try { File.Delete(item.GetTempFile()); } catch { }
            }
            finally
            {
                if (item.Status != DownloadStatus.Paused)
                {
                    Debug.WriteLine(item.Status);
                    ActiveDownloads.Remove(item);
                    _parallelLimiter.Release();
                }
            }
        });

        public ICommand CancelSendFileCommand => _cancelSendFileCommand ??= new RelayCommand(parameter => {
            if (parameter is FileModel file) {
                Debug.WriteLine("test");
                file.CancellationTokenSource?.Cancel();
                Conversations.LastOrDefault()?.Files?.Remove(file);
            }
        });

        private ICommand _cancelSendFileCommand;
        private ICommand _downloadFile;
        private ICommand _deleteFile;
        private ICommand _chooseFile;
        private ICommand _sendMesCommand;

        #endregion

        #endregion

        // Logic for search users
        #region DropDownListSearch

        #region Properties
        public List<User> users = new List<User>();
        public ObservableCollection<User> UserList
        {
            get => _userList;
            set
            {
                _userList = value;
                OnPropertyChanged(nameof(UserList));
            }
        }
        private ObservableCollection<User> _userList;
        #endregion

        #region Logics
        public async void LoadUserList()
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/selectuser"); // Send requers to server

            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(responseBody); // Deseriaize JSON-Object to collection

                if (jsonResponse is null) { return; }

                foreach (var user in jsonResponse) // Show users
                {
                    if (user.UserId == _userSession.CurrentUser.UserId) continue;
                    User userForList = _dtoConverter.GetUser(user);
                    users.Add(userForList);
                }
                UserList = new ObservableCollection<User>(users); // создаем пустую коллекцию
            }
            else
            {
                Console.WriteLine("Ошибка при получении пользователей");
            }
        }
        #endregion

        #endregion

        #region OpenedProfile

        #region Commands
        private ICommand _getUserProfileCommand;
        public ICommand GetUserProfileCommand => _getUserProfileCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is User user) // Get user
            {
                if (user != null)
                {
                    ProfileViewModel profileViewModel = new ProfileViewModel(_dtoConverter, _userSession)
                    {
                        statusThumbsUser = user
                    };
                    profileViewModel.OpenChatRequested += GetSelectedChatCommand.Execute;
                    UserProfile profile_window = new UserProfile(); // Create User's profile

                    profile_window.DataContext = profileViewModel; // Set DataContext
                    profile_window.Owner = Application.Current.MainWindow;
                    profile_window.Show();
                }
            }
        });
        #endregion

        #endregion

        #region DownloadArea

        #region Properties

        public bool DownloadAreaIsActive
        {
            get => _downloadAreaIsActive;
            set
            {
                _downloadAreaIsActive = value;
                OnPropertyChanged(nameof(DownloadAreaIsActive));
            }
        }
        private bool _downloadAreaIsActive;

        public ObservableCollection<DownloadItem> ActiveDownloads
        {
            get => _activeDownloads;
            set
            {
                _activeDownloads = value;
                OnPropertyChanged(nameof(ActiveDownloads));
            }
        }
        public ObservableCollection<DownloadItem> DownloadHistory
        {
            get => _downloadHistory;
            set
            {
                _downloadHistory = value;
                OnPropertyChanged(nameof(DownloadHistory));
            }
        }

        private ObservableCollection<DownloadItem> _activeDownloads = new();
        private ObservableCollection<DownloadItem> _downloadHistory = new();
        private SemaphoreSlim _parallelLimiter = new SemaphoreSlim(5); // Limit parallel download

        #endregion

        #region Logics
        private void LoadDownloadedArea()
        {
            if (System.IO.File.Exists("Properties/downloadHistory.json"))
            {
                var json = System.IO.File.ReadAllText("Properties/downloadHistory.json");
                if (!(json.Contains("{}") || json.Contains("[]")))
                {
                    List<DownloadItem> history = JsonConvert.DeserializeObject<List<DownloadItem>>(json);

                    foreach (DownloadItem item in history)
                    {
                        if (System.IO.File.Exists(item.SavePath)) {
                            DownloadHistory.Add(item);
                        }
                        else {
                            DeleteFileCommand.Execute(item);
                        } 
                    }
                }
            }
            else
            {
                System.IO.File.WriteAllText("Properties/downloadHistory.json", "{}");
            }
        }

        private void SaveToHistory(DownloadItem item)
        {
            DownloadHistory.Insert(0, item);

            var history = DownloadHistory.Take(100).ToList();
            var json = JsonConvert.SerializeObject(history);
            System.IO.File.WriteAllText("Properties/downloadHistory.json", json);
        }

        #endregion

        #region Commands

        public ICommand OpenDownloadedAreaCommand => _openDownloadedAreaCommand ??= new RelayCommand( _ =>
            { DownloadAreaIsActive = true; }
        );
        public ICommand CancelDownloadCommand => _cancelDownloadCommand ??= new RelayCommand(parameter => {
            if (parameter is DownloadItem item) {
                if (item.Status == DownloadStatus.Paused) {
                    item.Status = DownloadStatus.Cancelled;
                    DownloadFile.Execute(item);
                    return;
                }
                item.Status = DownloadStatus.Cancelled;
                item.CancellationTokenSource.Cancel();
            }
        });
        public ICommand OpenFileCommand => _openFileCommand ??= new RelayCommand(parameter => {
            if (parameter is DownloadItem item) {
                Process.Start(new ProcessStartInfo(item.SavePath) { UseShellExecute = true });
            }
        });
        public ICommand ShowInFolderCommand => _showInFolderCommand ??= new RelayCommand(parameter => {
            if (parameter is DownloadItem item) {
                if (File.Exists(item.SavePath)) {
                    Process.Start("explorer.exe", $"/select, \"{item.SavePath}\"");
                }
            }
        });
        public ICommand DeleteFileCommand => _deleteFileCommand ??= new RelayCommand(async parameter =>
        {
            if (parameter is DownloadItem item) {
                try {
                    DownloadHistory.Remove(item);
                    var json = JsonConvert.SerializeObject(DownloadHistory);
                    await System.IO.File.WriteAllTextAsync("Properties/downloadHistory.json", json);
                }
                catch (Exception ex) {
                    Debug.WriteLine($"Error deleting file: {ex.Message}");
                }
            }
        });
        public ICommand OpenDownloadFolder => _openDownloadFolder ??= new RelayCommand(parameter =>
        {
            string? savePath = ((App)Application.Current).Configuration["AppSettings:SavePath"];
            
            if (Directory.Exists(savePath)) {
                Process.Start("explorer.exe", savePath);
            }
        });
        public ICommand PauseDownloadCommand => _pauseDownloadCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is DownloadItem item && item.Status == DownloadStatus.Downloading)
            {
                item.Status = DownloadStatus.Paused;
                item.CancellationTokenSource?.Cancel();
            }
        });
        public ICommand ResumeDownloadCommand => _resumeDownloadCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is DownloadItem item && item.Status == DownloadStatus.Paused)
            {
                item.Status = DownloadStatus.Downloading;
                item.CancellationTokenSource = new CancellationTokenSource();
                DownloadFile.Execute(item);
            }
        });


        private ICommand _pauseDownloadCommand;
        private ICommand _resumeDownloadCommand;
        private ICommand _openDownloadFolder;
        private ICommand _deleteFileCommand;
        private ICommand _showInFolderCommand;
        private ICommand _openFileCommand;
        private ICommand _cancelDownloadCommand;
        private ICommand _openDownloadedAreaCommand;

        #endregion

        #endregion

        public ICommand LogoutCommand => _logoutCommand ??= new RelayCommand( async _ =>
        {
            await _authService.LogoutAsync();

            AuthWindow auth = App.ServiceProvider.GetRequiredService<AuthWindow>();
            Application.Current.MainWindow = auth;
            Application.Current.Windows.OfType<MainWindow>().FirstOrDefault()?.Close();
            auth.Show();
        });
        private ICommand _logoutCommand;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }    
}