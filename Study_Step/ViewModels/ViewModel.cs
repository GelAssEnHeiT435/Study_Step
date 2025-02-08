using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Commands;
using Study_Step.Models;
using Study_Step.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Study_Step.ViewModels
{
    public class ViewModel: INotifyPropertyChanged
    {
        public static readonly HttpClient client = new HttpClient();
        public static string apiUrl = "http://localhost:5000/api/chat";
        #region MainWindow

        // Параметры для отображения текущего выделенного чата
        #region Properties
        public string ContactName { get; set; }
        public BitmapImage bitmapPhoto { get; set; }
        public string LastSeen { get; set; }
        public bool ChatIsActive 
        {
            get => _ChatIsActive;
            set
            {
                _ChatIsActive = value;
                OnPropertyChanged("ChatIsActive");
            }
        }
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
                var jsonResponse = JsonConvert.DeserializeObject<IEnumerable<Chat>>(responseBody);

                Chats = new ObservableCollection<Chat>(); // создаем пустую коллекцию

                // Отображаем полученные чаты
                foreach (var chat in jsonResponse)
                {
                    if (chat.ContactPhoto != null)
                    {
                        // Преобразуем массив байтов в BitmapImage
                        BitmapImage bitmapImage = ConvertByteArrayToBitmapImage(chat.ContactPhoto);

                        // Присваиваем BitmapImage в свойство ContactPhoto (или можно использовать в ImageBrush)
                        chat.bitmapPhoto = bitmapImage;
                    }

                    Chats.Add(chat);
                    System.Diagnostics.Debug.WriteLine($"Name: {chat.Name} Chat ID: {chat.Message}, Chat Name: {chat.LastMessageTime}");
                }
            }
            else
            {
                Console.WriteLine("Ошибка при получении чатов");
            }
            
            OnPropertyChanged("Chats");

        }
        #endregion

        // Команды для отображения информации о чате при его выборе
        #region Commands
        private ICommand _getSelectedChatCommand;
        public ICommand GetSelectedChatCommand => _getSelectedChatCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is Chat v) // получение имени и фото пользователя при выборе чата
            {
                // Отображение заголовка чата с передачей данных
                ChatIsActive = true;
                System.Diagnostics.Debug.WriteLine(ChatIsActive);

                ContactName = v.Name;
                OnPropertyChanged("ContactName");

                bitmapPhoto = v.bitmapPhoto;
                OnPropertyChanged("bitmapPhoto");

                // загрузка сообщений конкретного чата
                Application.Current.Properties["ChatId"] = v.Id;
                LoadChatConversation(v.Id);
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
                OnPropertyChanged("Conversations");
            }
        }
        protected ObservableCollection<Message> mConversations;

        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged("MessageText");
            }
        }
        protected string messageText { get; set; }
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
                    System.Diagnostics.Debug.WriteLine(mes.Text);
                    System.Diagnostics.Debug.WriteLine(mes.IsOutside);
                }

                OnPropertyChanged("Conversations");
            }
            else
            {
                Console.WriteLine("Ошибка при получении чатов");
            }
        }
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

        public ViewModel()
        {
            LoadChats();
            LoadUserList();

            ChatIsActive = false;
        }

        // Метод для преобразования массива байт в BitmapImage
        private BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
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
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }
}
