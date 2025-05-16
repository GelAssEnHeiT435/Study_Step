using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;
using Study_Step.Commands;
using Study_Step.Interfaces;
using Study_Step.Models;
using Study_Step.Models.DTO;
using Study_Step.Services;
using Study_Step.UI.Windows;
using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
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
            _signalRService.AddNewUserEvent += AddNewUser;
            _signalRService.OnMessageReceivedEvent += MessageReceived;
            _signalRService.OnUpdateIdEvent += UpdateIdLastSendMes;
            _signalRService.DeletionMessageEvent += DeletionMessage;
            _signalRService.EditableMessageEvent += EditableMessage;
            _signalRService.ReadingMessageEvent += ReadingMessage;

            LoadChats();
            LoadUserList();
            LoadDownloadedArea();

            UserPhoto = _userSession.CurrentUser?.bitmapPhoto;

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

        public BitmapImage? UserPhoto {
            get => _userPhoto;
            set{
                _userPhoto = value;
                OnPropertyChanged(nameof(UserPhoto));
            }
        }

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

        private BitmapImage? _userPhoto;
        private string _contactName;
        private BitmapImage _bitmapPhoto;
        private string _lastSeen;
        private bool _ChatIsActive;

        #endregion

        #endregion

        #region Chat List

        #region Properties

        public ObservableCollection<Chat> Chats { get; set; }
        public Chat? CurrentChat { get; set; }

        #endregion

        #region Logics
        private async void LoadChats()
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5000/api/chat/getallchats?userId={_userSession.CurrentUser.UserId}"); // Send Request to Server

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
                    Debug.WriteLine(thisChat.UnreadCount);
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
            var existingChat = Chats.FirstOrDefault(chat => chat.ChatId == updatedChat.ChatId);

            if (existingChat != null)
            {
                Chats = new ObservableCollection<Chat>(Chats.OrderByDescending(c => c.LastMessageTime));
                OnPropertyChanged(nameof(Chats));
            }
            else
            {
                Chats.Insert(0, updatedChat);
            }
        }

        #endregion

        #region Commands
        
        public ICommand GetSelectedChatCommand => _getSelectedChatCommand ??= new RelayCommand(async parameter =>
        {
            if (parameter is Chat chat) // получение имени и фото пользователя при выборе чата
            {
                // Отображение заголовка чата с передачей данных
                ChatIsActive = true;
                CurrentChat = chat;

                Chat? choosenChat = Chats.Where(c => c.IsChoosen).FirstOrDefault();
                if (choosenChat != null)
                    choosenChat.IsChoosen = false;
                CurrentChat.IsChoosen = true;

                ContactName = CurrentChat.Name;
                bitmapPhoto = CurrentChat.bitmapPhoto;
                FilesListObject = new ObservableCollection<FileModel>();

                CurrentChat.UnreadCount = 0;
                await _signalRService.ReadMessageAsync(CurrentChat.UserId_InChat.ToString(), CurrentChat.ChatId);

                LoadChatConversation(chat.ChatId);
            }
        });
        private ICommand _getSelectedChatCommand;

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
            var span = DateTime.Now.ToLocalTime() - time;
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


        private Message? _selectedMessage { get; set; }
        private Chat? _selectedChat { get; set; }
        private bool _isEditMode;

        #endregion

        #region Logics

        private async void LoadChatConversation(int Id)
        {
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5000/api/chat/messages?userId={_userSession.CurrentUser?.UserId}&chatId={Id}"); // Send request to server

            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync(); // Read response
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<MessageDTO>>(responseBody); // Deserialize JSON to collection
                IEnumerable<Message> messages = _dtoConverter.GetMessageList(jsonResponse);

                Conversations = new ObservableCollection<Message>();

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

        private async void SendNewMessage()
        {
            if (MessageText.IsNullOrWhiteSpace() && FilesListObject.Count == 0) return;

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
            FilesListObject.Clear();

            // add file to response
            foreach (FileModel file in messageChat.Files.ToList())
            {
                try
                {
                    await _fileService.SendFileAsync(file);
                }
                catch (OperationCanceledException)
                {
                    file.Status = SendingStatus.Cancelled;
                    return;
                }
                catch (Exception ex)
                {
                    messageChat.Files.Remove(file);
                    Debug.WriteLine(ex);
                    return;
                }
                finally
                {
                    file.CancellationTokenSource?.Dispose();
                    file.CancellationTokenSource = null;

                    if (string.IsNullOrWhiteSpace(messageChat.Text) & !messageChat.Files.Any())
                    {
                        Conversations.Remove(messageChat);
                    }
                }
            }

            try
            {
                if (messageChat.Text == null && messageChat.Files.Any())
                    CurrentChat.LastMessage = "файл";
                else
                    CurrentChat.LastMessage = messageChat.Text;

                CurrentChat.LastMessageTime = messageChat.SentAt;
                ChatDTO chatObject = _dtoConverter.GetChatDTO(CurrentChat);
                MessageDTO messageObject = _dtoConverter.GetMessageDTO(messageChat);

                var userDto = _userSession.GetCurrentUserDTO();
                chatObject.ContactPhoto = userDto.ContactPhoto;
                chatObject.Name = userDto.Username;

                UpdateChatList(CurrentChat);

                await _signalRService.SendMessageAsync(CurrentChat.UserId_InChat.ToString(), chatObject, messageObject);
            }
            catch (HubException hex)
            {
                Debug.WriteLine($"{hex.Message}");
            }
        }

        private async void SendEditableMessage()
        {
            if (new HashSet<FileModel>(_selectedMessage.Files).SetEquals(FilesListObject) && 
                _selectedMessage.Text == MessageText) return;
            if (string.IsNullOrWhiteSpace(MessageText) && !FilesListObject.Any()) return;

            _selectedMessage.Text = MessageText;
            _selectedMessage.Files = new ObservableCollection<FileModel>(FilesListObject.ToList());

            MessageText = string.Empty;
            FilesListObject.Clear();

            foreach (FileModel file in _selectedMessage.Files.Where(m => m.MessageId == 0).ToList()) {
                try
                {
                    await _fileService.SendFileAsync(file);
                }
                catch (OperationCanceledException)
                {
                    file.Status = SendingStatus.Cancelled;
                    return;
                }
                catch (Exception ex)
                {
                    _selectedMessage.Files.Remove(file);
                    Debug.WriteLine(ex);
                    return;
                }
                finally
                {
                    file.CancellationTokenSource?.Dispose();
                    file.CancellationTokenSource = null;
                }
            }

            Message? lastMessage = Conversations?.LastOrDefault();
            if (lastMessage?.MessageId == _selectedMessage.MessageId)
            {
                if (_selectedMessage.Text == null && _selectedMessage.Files.Any())
                    CurrentChat.LastMessage = "файл";
                else
                    CurrentChat.LastMessage = _selectedMessage.Text;
            }
            ChatDTO chatDTO = _dtoConverter.GetChatDTO(CurrentChat);
            MessageDTO messageDTO = _dtoConverter.GetMessageDTO(_selectedMessage);

            var userDto = _userSession.GetCurrentUserDTO();
            chatDTO.ContactPhoto = userDto.ContactPhoto;
            chatDTO.Name = userDto.Username;

            try
            {
                _signalRService.EditMessageAsync(CurrentChat.UserId_InChat.ToString(), chatDTO, messageDTO);
            }
            catch (HubException hex)
            {
                Debug.WriteLine(hex.Message);
            }
            finally
            {
                _selectedMessage = null;
                _isEditMode = false;
            }
        }

        #endregion

        #region ReceiverLogic

        private void MessageReceived(string sender, ChatDTO chat, MessageDTO message)
        {
            Message messageObject = _dtoConverter.GetMessage(message);
            Chat chatObject = _dtoConverter.GetChat(chat);

            UpdateChatList(chatObject);

            Chat recieverChat = Chats.FirstOrDefault(x => x.ChatId == chatObject.ChatId);

            recieverChat.LastMessage = chatObject.LastMessage;
            recieverChat.LastMessageTime = chatObject.LastMessageTime;
            UpdateMessage(recieverChat);

            if (CurrentChat != null && CurrentChat.ChatId == recieverChat.ChatId) // if a chat is open with the person who sent the message
            {
                messageObject.IsOutside = true; // changing the sender's side
                Conversations.Add(messageObject);
                ScrollToEnd?.Invoke();
            }
            else if (CurrentChat == null || CurrentChat.ChatId != recieverChat.ChatId) // If a chat with another person is open or the chat is not open
            {
                recieverChat.UnreadCount++;
            }
            else // If the chats are not open
            {
                // TODO: add notify system
            }
        }

        private void UpdateIdLastSendMes(string sender, ChatDTO chat, MessageDTO message)
        {
            Message messageObject = _dtoConverter.GetMessage(message);
            Chat chatObject = _dtoConverter.GetChat(chat);

            Message? mes = Conversations?.Where(m => m.UserId == int.Parse(sender)).LastOrDefault();
            if (int.Parse(sender) == _userSession.CurrentUser.UserId && mes != null)
            {
                mes.MessageId = messageObject.MessageId;
                mes.Files = messageObject.Files;
            }
            UpdateMessage(CurrentChat);
        }

        private void DeletionMessage(ChatDTO chatDTO, MessageDTO messageDTO)
        {
            Chat chat = _dtoConverter.GetChat(chatDTO);
            if (CurrentChat != null && CurrentChat.ChatId == chatDTO.ChatId)
            {
                Message? message = Conversations?.Where(m => m.MessageId == messageDTO.MessageId).FirstOrDefault();
                if (message != null)
                    Conversations?.Remove(message);
            }
            Chat? extChat = Chats.Where(c => c.ChatId == chatDTO.ChatId).FirstOrDefault();
            extChat.LastMessage = chat.LastMessage;
            extChat.LastMessageTime = chat.LastMessageTime;
            UpdateMessage(extChat);
            UpdateChatList(extChat);
        }

        private void EditableMessage(ChatDTO chatDTO, MessageDTO messageDTO)
        {
            Chat chat = _dtoConverter.GetChat(chatDTO);
            Message message = _dtoConverter.GetMessage(messageDTO);
            if (CurrentChat != null && CurrentChat.ChatId == chatDTO.ChatId)
            {
                Message? thisMessage = Conversations?.Where(m => m.MessageId == messageDTO.MessageId).FirstOrDefault();
                if (message != null)
                {
                    thisMessage.Text = message.Text;
                    thisMessage.Files = message.Files;
                }
            }
            Chat? extChat = Chats.Where(c => c.ChatId == chatDTO.ChatId).FirstOrDefault();
            extChat.LastMessage = chat.LastMessage;
            extChat.LastMessageTime = chat.LastMessageTime;
        }

        private void ReadingMessage(int chatId)
        {
            if (CurrentChat != null && Conversations != null && CurrentChat.ChatId == chatId)
            {
                foreach(Message message in Conversations.Where(m => !m.isRead))
                    message.isRead = true;
            }
        }

        #endregion

        #region Commands

        public ICommand SendMesCommand => _sendMesCommand ??= new RelayCommand(_ =>
        {
            if (_isEditMode) SendEditableMessage();
            else SendNewMessage();
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
                foreach (var filePath in openFileDialog.FileNames) 
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
                if (item.Status == DownloadStatus.Cancelled)
                    try { File.Delete(item.GetTempFile()); } catch {}
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
                    ActiveDownloads.Remove(item);
                    _parallelLimiter.Release();
                }
            }
        });

        public ICommand CancelSendFileCommand => _cancelSendFileCommand ??= new RelayCommand(parameter => {
            if (parameter is FileModel file) {
                file.CancellationTokenSource?.Cancel();
                Conversations.LastOrDefault()?.Files?.Remove(file);
            }
        });

        public ICommand DeleteMessageCommand => _deleteMessageCommand ??= new RelayCommand(async _ =>
        {
            Message? lastMessage = Conversations?.LastOrDefault();
            Conversations?.Remove(_selectedMessage);

            if (lastMessage?.MessageId == _selectedMessage.MessageId)
            {
                Message? newlastMessage = Conversations?.LastOrDefault();
                CurrentChat.LastMessageTime = newlastMessage.SentAt;

                if (newlastMessage.Text == null && newlastMessage.Files.Any())
                    CurrentChat.LastMessage = "файл";
                else
                    CurrentChat.LastMessage = newlastMessage.Text;

                UpdateMessage(CurrentChat);
                UpdateChatList(CurrentChat);
            }

            ChatDTO chatDTO = _dtoConverter.GetChatDTO(CurrentChat);
            MessageDTO messageDTO = _dtoConverter.GetMessageDTO(_selectedMessage);

            var userDto = _userSession.GetCurrentUserDTO();
            chatDTO.ContactPhoto = userDto.ContactPhoto;
            chatDTO.Name = userDto.Username;

            try
            {
                await _signalRService.DeleteMessageAsync(CurrentChat.UserId_InChat.ToString(), chatDTO,
                                                         messageDTO, _isDeleteForAll);
            }
            catch (HubException hex)
            {
                Debug.WriteLine($"{hex.Message}");
            }
            finally
            {
                _selectedMessage = null;
                CloseDeletionWindowCommand.Execute(null);
            }
        });


        private ICommand _cancelSendFileCommand;
        private ICommand _downloadFile;
        private ICommand _deleteFile;
        private ICommand _chooseFile;
        private ICommand _sendMesCommand;
        private ICommand _deleteMessageCommand;

        #endregion

        #endregion

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
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5000/api/chat/selectuser"); // Send requers to server

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

        public void AddNewUser(UserDTO userDTO)
        {
            User user = _dtoConverter.GetUser(userDTO);
            if (user != null && !users.Any(u => u.UserId == user.UserId))
                users.Add(user);
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
                try {
                    Process.Start(new ProcessStartInfo(item.SavePath) { UseShellExecute = true });
                }
                catch (Win32Exception) {
                    DownloadHistory.Remove(item);
                }
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

        #region OptionalPopup

        #region Properties

        public bool IsDeleteForAll
        {
            get => _isDeleteForAll;
            set
            {
                _isDeleteForAll = value;
                OnPropertyChanged(nameof(IsDeleteForAll));
            }
        }
        private bool _isDeleteForAll;

        #endregion

        #region Commands

        public ICommand OpenOptChatCommand => _openOptChatCommand ??= new RelayCommand( parameter =>
        {
            if (parameter is Chat thisChat)
            {
                foreach (var chat in Chats.Where(c => c.ChatId != thisChat.ChatId)) {
                    chat.IsPopupOpen = false;
                }
                thisChat.IsPopupOpen = true;
                _selectedChat = thisChat;

                CommandManager.InvalidateRequerySuggested();
            }
        });

        public ICommand OpenOptMessageCommand => _openOptMessageCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is Message thisMessage)
            {
                foreach (var mes in Conversations.Where(m => m.MessageId != thisMessage.MessageId)) {
                    mes.IsPopupOpen = false;
                }
                thisMessage.IsPopupOpen = true;
                _selectedMessage = thisMessage;

                CommandManager.InvalidateRequerySuggested();
            }
        });
        
        public ICommand OpenDeletionWindowCommand => _openDeletionWindowCommand ??= new RelayCommand(_ =>
        {
            _selectedMessage.IsPopupOpen = false;
            DeletionWindow dWindow = App.ServiceProvider.GetRequiredService<DeletionWindow>();
            dWindow.Owner = App.ServiceProvider.GetRequiredService<MainWindow>();
            dWindow.ShowDialog();
        });

        public ICommand EditMessageCommand => _editMessageCommand ??= new RelayCommand(_ =>
        {
            _isEditMode = true;
            _selectedMessage.IsPopupOpen = false;
            MessageText = _selectedMessage?.Text;
            FilesListObject = new ObservableCollection<FileModel>(_selectedMessage?.Files);
        });

        public ICommand DeleteChatCommand => _deleteChatCommand ??= new RelayCommand(async _ =>
        {
            ChatIsActive = false;
            OnPropertyChanged(nameof(ChatIsActive));

            HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5000/api/chat/deletechat?" +
                                                                    $"userId={_userSession.CurrentUser.UserId}&" +
                                                                    $"chatId={_selectedChat?.ChatId}");
            if (response.IsSuccessStatusCode)
                Chats.Remove(_selectedChat);
            else
                Debug.WriteLine(response.RequestMessage);

            CurrentChat = null;
            Conversations = null;
        });

        public ICommand CloseDeletionWindowCommand => _closeDeletionWindowCommand ??= new RelayCommand(_ =>{   
            Application.Current.Windows.OfType<DeletionWindow>().FirstOrDefault()?.Close();
        });

        private ICommand _openOptMessageCommand;
        private ICommand _openOptChatCommand;
        private ICommand _openDeletionWindowCommand;
        private ICommand _closeDeletionWindowCommand;
        private ICommand _editMessageCommand;
        private ICommand _deleteChatCommand;

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