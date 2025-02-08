using Newtonsoft.Json;
using Study_Step.ViewModels;
using Study_Step.Models;
using Study_Step.Models.DTO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Study_Step
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "http://localhost:5000/api/chat";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ViewModel viewModel = (ViewModel)DataContext;

            // Проверка, если была нажата клавиша Escape
            if (e.Key == Key.Escape)
            {
                viewModel.ChatIsActive = false;
            }
        }

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            string? message = MessageBox.Text;
            ViewModel viewModel = (ViewModel)DataContext;

            if (message == null || message.Length == 0) return;

            MessageDTO mes = new MessageDTO()
            {
                SenderId = (int)Application.Current.Properties["Id"],
                ChatId = (int)Application.Current.Properties["ChatId"],
                Text = message,
                Type = MessageType.Text,
                FileUrl = null
            };

            // Преобразуем объект пользователя в JSON строку
            string json = JsonConvert.SerializeObject(mes);

            // Создаем HTTP запрос
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Отправляем запрос на сервер
            HttpResponseMessage response = await client.PostAsync("http://localhost:5000/api/chat/newmessage", content);
            if (response.IsSuccessStatusCode)
            {
                Message sentMes = new Message()
                {
                    SenderId = (int)Application.Current.Properties["Id"],
                    ChatId = (int)Application.Current.Properties["ChatId"],
                    Text = message,
                    SentAt = DateTime.UtcNow,
                    Type = MessageType.Text,
                    IsOutside = false,
                    FileUrl = null
                };

                viewModel.Conversations.Add(sentMes);
                MessageBox.Text = "";
                MessageBox.Focus();
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Ошибка: {error}");
            }
        }
    }
}