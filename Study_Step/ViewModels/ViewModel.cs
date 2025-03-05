using Newtonsoft.Json;
using Study_Step.Commands;
using Study_Step.Interfaces;
using Study_Step.Models;
using Study_Step.Models.DTO;
using Study_Step.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Study_Step.ViewModels
{
    public class ViewModel: INotifyPropertyChanged
    {
        public static readonly HttpClient client = new HttpClient();
        public static string apiUrl = "http://localhost:5000/api/chat";

        private readonly SignalRService _signalRService;
        private readonly IImageService _imageService;
        private readonly DtoConverterService _dtoConverter;

        public ViewModel(SignalRService signalRService, 
                         IImageService imageService,
                         DtoConverterService dtoConverter)
        {
            _signalRService = signalRService;
            _imageService = imageService;
            _dtoConverter = dtoConverter;

            // Add EventsListeners
            _signalRService.OnMessageReceived += MessageReceived;

            LoadChats();
            LoadUserList();

            ChatIsActive = false;
        }
        #region MainWindow

        #region Events

        public event Action ScrollToEnd;

        #endregion

        // Параметры для отображения текущего выделенного чата
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

        // Отображение списка чатов пользователя
        #region Chat List
        #region Properties
        public ObservableCollection<Chat> Chats { get; set; }
        #endregion

        #region Logics
        public async void LoadChats()
        {
            string json = JsonConvert.SerializeObject(Application.Current.Properties["Id"]); // Convert User's Id to JSON-Object
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // Create HTTP-Request
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/getallchats?userId={Application.Current.Properties["Id"]}"); // Send Request to Server

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync(); // Read body response
                var jsonResponse = JsonConvert.DeserializeObject<UserChatsResponse>(responseBody); // Deserialize object to collection

                Chats = new ObservableCollection<Chat>();
                
                foreach (var chat in jsonResponse.Chats)
                {
                    Chat thisChat = _dtoConverter.GetChat(chat); // convert ChatDTO to Chat
                    foreach (var uc in jsonResponse.UserChats) // Get User's Id to send messages
                    {
                        if (uc.UserId != (int)Application.Current.Properties["Id"] && chat.ChatId == uc.ChatId) {
                            thisChat.UserId_InChat = uc.UserId;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(thisChat.ChatId);
                    Chats.Add(thisChat);
                }
            }
            else
            {
                Console.WriteLine("Ошибка при получении чатов");
            }
            OnPropertyChanged(nameof(Chats));
        }
        #endregion

        // Команды для отображения информации о чате при его выборе
        #region Commands
        private ICommand _getSelectedChatCommand;
        public ICommand GetSelectedChatCommand => _getSelectedChatCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is Chat chat) // получение имени и фото пользователя при выборе чата
            {
                // Отображение заголовка чата с передачей данных
                ChatIsActive = true;

                ContactName = chat.Name;
                bitmapPhoto = chat.bitmapPhoto;

                // загрузка сообщений конкретного чата
                System.Diagnostics.Debug.WriteLine(chat.ChatId);

                Application.Current.Properties["ChatId"] = chat.ChatId;
                Application.Current.Properties["RecieverId"] = chat.UserId_InChat;
                LoadChatConversation(chat.ChatId);
            }
        });
        #endregion

        #endregion

        // Отображение истории сообщений при открытии чата
        #region ChatConversation

        #region Properties
        public ObservableCollection<Message> Conversations 
        {   get => mConversations;
            set
            {
                mConversations = value;
                OnPropertyChanged(nameof(Conversations));
            }
        }
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }
        private ObservableCollection<Message> mConversations;
        private string _messageText { get; set; }
        #endregion

        #region Logics
        private async void LoadChatConversation(int Id)
        {
            string json = JsonConvert.SerializeObject(Id); // Convert Id to JSON-Object
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // Create HTTP-Request
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/messages?chatId={Id}"); // Send request to server

            if (response.IsSuccessStatusCode) {
                string responseBody = await response.Content.ReadAsStringAsync(); // Read response
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<MessageDTO>>(responseBody); // Deserialize JSON to collection
                IEnumerable<Message> messages = _dtoConverter.GetMessageList(jsonResponse);

                Conversations = new ObservableCollection<Message>();

                // Отображаем полученные чаты
                foreach (var mes in messages)
                {
                    mes.IsOutside = mes.UserId != (int) Application.Current.Properties["Id"];
                    Conversations.Add(mes);
                    ScrollToEnd?.Invoke();
                }
            }
            else
            {
                Console.WriteLine("Ошибка при получении чатов");
            }
        }

        private void MessageReceived(string sender, MessageDTO message)
        {
            Message messageObject = _dtoConverter.GetMessage(message);

            messageObject.IsOutside = true; // changing the sender's side
            Conversations.Add(messageObject);

            ScrollToEnd?.Invoke();
        }
        #endregion

        #region Commands

        private ICommand _sendMesCommand;
        public ICommand SendMesCommand => _sendMesCommand ??= new RelayCommand(async parameter =>
        {
            if (!ChatIsActive) { return; }

            Message messageChat = new Message()
            {
                UserId = (int)Application.Current.Properties["Id"],
                ChatId = (int)Application.Current.Properties["ChatId"],
                Text = MessageText,
                SentAt = DateTime.UtcNow,
                Type = MessageType.Text,
            };

            MessageDTO messageObject = _dtoConverter.GetMessageDTO(messageChat);
            await _signalRService.SendMessageAsync(Application.Current.Properties["RecieverId"].ToString(),
                                                   messageObject);

            messageChat.IsOutside = false;
            Conversations.Add(messageChat);
            ScrollToEnd?.Invoke();

            MessageText = string.Empty;
        });

        #endregion


        #endregion

        // Логика поиска пользователей в системе
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

                foreach (var user in jsonResponse) // Show users
                {
                    if (user.Username.Equals( Application.Current.Properties["Username"] )) continue;
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
                    ProfileViewModel profileViewModel = new ProfileViewModel {
                        statusThumbsUser = user
                    };
                    UserProfile profile_window = new UserProfile(); // Create User's profile

                    profile_window.DataContext = profileViewModel; // Set DataContext
                    profile_window.Owner = Application.Current.MainWindow;
                    profile_window.Show();
                }
            }
        });
        #endregion

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }
}
