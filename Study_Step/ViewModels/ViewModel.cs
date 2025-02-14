using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Commands;
using Study_Step.Models;
using Study_Step.Models.DTO;
using Study_Step_Server.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Study_Step.ViewModels
{
    public class ViewModel: INotifyPropertyChanged
    {
        public static readonly HttpClient client = new HttpClient();
        public static string apiUrl = "http://localhost:5000/api/chat";

        private readonly SignalRService _signalRService;

        public ViewModel(SignalRService signalRService)
        {
            _signalRService = signalRService;
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
            // Преобразуем объект пользователя в JSON строку
            string json = JsonConvert.SerializeObject(Application.Current.Properties["Id"]);

            // Создаем HTTP запрос
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            // Отправляем запрос на сервер
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/user_chats?userId={Application.Current.Properties["Id"]}");

            if (response.IsSuccessStatusCode)
            {
                // Если запрос успешен, читаем тело ответа
                string responseBody = await response.Content.ReadAsStringAsync();

                // Десериализуем JSON в коллекцию чатов
                var jsonResponse = JsonConvert.DeserializeObject<UserChatsResponse>(responseBody);

                Chats = new ObservableCollection<Chat>(); // создаем пустую коллекцию

                // Отображаем полученные чаты
                foreach (var chat in jsonResponse.Chats)
                {
                    Chat thisChat = new Chat()
                    {
                        Id = chat.Id,
                        Name = chat.Name,
                        Message = chat.Message,
                        LastMessageTime = chat.LastMessageTime,
                        Type = chat.Type
                    };

                    if (chat.ContactPhoto != null)
                    {
                        // Преобразуем массив байтов в BitmapImage
                        BitmapImage bitmapImage = ConvertByteArrayToBitmapImage(chat.ContactPhoto);

                        // Присваиваем BitmapImage в свойство ContactPhoto (или можно использовать в ImageBrush)
                        thisChat.bitmapPhoto = bitmapImage;
                    }

                    foreach (var uc in jsonResponse.UserChats)
                    {
                        if (uc.UserId != (int)Application.Current.Properties["Id"] && chat.Id == uc.ChatId)
                        {
                            thisChat.UserId_InChat = uc.UserId;
                        }
                        
                    }
                    System.Diagnostics.Debug.WriteLine(thisChat.UserId_InChat);
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
                Application.Current.Properties["ChatId"] = chat.Id;
                Application.Current.Properties["RecieverId"] = chat.UserId_InChat;
                LoadChatConversation(chat.Id);
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
        async void LoadChatConversation(int Id)
        {
            // Преобразуем объект пользователя в JSON строку
            string json = JsonConvert.SerializeObject(Id);

            // Создаем HTTP запрос
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            // Отправляем запрос на сервер
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/messeges?chatId={Id}");

            if (response.IsSuccessStatusCode)
            {
                // Если запрос успешен, читаем тело ответа
                string responseBody = await response.Content.ReadAsStringAsync();

                // Десериализуем JSON в коллекцию чатов
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<Message>>(responseBody);

                Conversations = new ObservableCollection<Message>(); // создаем пустую коллекцию

                // Отображаем полученные чаты
                foreach (var mes in jsonResponse)
                {
                    mes.IsOutside = mes.SenderId != (int)Application.Current.Properties["Id"];
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
            Message messageObject = new Message()
            {
                SenderId = message.SenderId,
                ChatId = message.ChatId,
                Text = message.Text,
                SentAt = message.SentAt,
                Type = message.Type,
                IsOutside = true
            };
            Conversations.Add(messageObject);
            ScrollToEnd?.Invoke();
        }
        #endregion

        #region Commands

        private ICommand _sendMesCommand;
        public ICommand SendMesCommand => _sendMesCommand ??= new RelayCommand(async parameter =>
        {
            if (!ChatIsActive) { return; }

            MessageDTO messageObject = new MessageDTO()
            {
                SenderId = (int)Application.Current.Properties["Id"],
                ChatId = (int)Application.Current.Properties["ChatId"],
                Text = MessageText,
                SentAt = DateTime.UtcNow,
                Type = MessageType.Text,
            };
            Message messageChat = new Message()
            {
                SenderId = messageObject.SenderId,
                ChatId = messageObject.ChatId,
                Text = MessageText,
                SentAt = messageObject.SentAt,
                Type = MessageType.Text,
                IsOutside = false
            };

            Conversations.Add(messageChat);
            ScrollToEnd?.Invoke();

            await _signalRService.SendMessageAsync(Application.Current.Properties["RecieverId"].ToString(),
                                                   messageObject);
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
                OnPropertyChanged("UserList");
            }
        }
        private ObservableCollection<User> _userList;
        #endregion

        #region Logics
        public async void LoadUserList()
        {
            // Отправляем запрос на сервер
            HttpResponseMessage response = await client.GetAsync(apiUrl + $"/selectuser");

            if (response.IsSuccessStatusCode)
            {
                // Если запрос успешен, читаем тело ответа
                string responseBody = await response.Content.ReadAsStringAsync();

                // Десериализуем JSON в коллекцию чатов
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<UserDTO>>(responseBody);

                // Отображаем полученных пользователей
                foreach (var user in jsonResponse) 
                {
                    if (user.Username.Equals( Application.Current.Properties["Username"] )) continue;
                    users.Add(new User()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Photo = ConvertByteArrayToBitmapImage(user.ContactPhoto),
                        Email = user.Email,
                        Status = user.Status,
                        LastLogin = user.LastLogin
                    });
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
            if (parameter is User user) // получение имени и фото пользователя при выборе чата
            {
                if (user != null)
                {
                    // Создайте новый экземпляр ViewModel для нового окна
                    ProfileViewModel profileViewModel = new ProfileViewModel
                    {
                        statusThumbsUser = user
                    };

                    // Создаем окно профиля
                    Profile profile_window = new Profile();

                    // Устанавливаем DataContext для нового окна профиля
                    profile_window.DataContext = profileViewModel; // Привязываем новый ViewModel

                    // Теперь MainWindow — главное окно для taskWindow
                    profile_window.Owner = Application.Current.MainWindow;

                    // Показываем окно
                    profile_window.Show();
                }
            }
        });
        #endregion

        #endregion

        // Метод для преобразования массива байт в BitmapImage
        private BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray is null) return null;

            BitmapImage bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream(byteArray))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }

        private BitmapImage LoadImage(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);  // Путь к файлу
            bitmap.EndInit();
            return bitmap;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }
}
