using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Models;
using Study_Step.ViewModels;
using Study_Step.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Study_Step.Pages
{
    /// <summary>
    /// Логика взаимодействия для SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiUrl = "http://localhost:5000/api/auth";
        public SignInPage()
        {
            InitializeComponent();
        }

        private void ToRegisterPage(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new RegisterPage());

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            // Получаем пароль из PasswordBox и передаем его в ViewModel
            PasswordBox passwordBox = (PasswordBox)sender;
            AuthViewModel viewModel = (AuthViewModel)DataContext;
            viewModel.Password = passwordBox.Password;
        }

        private async void UserLogin(object sender, RoutedEventArgs e)
        {
            AuthViewModel viewModel = (AuthViewModel)DataContext;

            AuthUser user = new AuthUser
            {
                Email = viewModel.Email,
                Password = viewModel.Password
            };

            // Преобразуем объект пользователя в JSON строку
            string json = JsonConvert.SerializeObject(user);

            // Создаем HTTP запрос
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Отправляем запрос на сервер
                HttpResponseMessage response = await client.PostAsync(apiUrl + "/login", content);

                // Проверяем статус ответа
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Авторизация прошла успешно!");

                    //// Получаем JWT токен и идентификатор пользователя из ответа


                    //string token = jsonResponse["token"].ToString();
                    //int userId = jsonResponse["userId"].ToObject<int>();

                    //// Получаем время истечения срока действия токена
                    //DateTime expiration = DateTime.UtcNow.AddMinutes(60); // Примерное время истечения (можно адаптировать)

                    //// Создаем объект UserSession
                    //UserSession userSession = new UserSession
                    //{
                    //    UserId = userId,
                    //    Token = token,
                    //    Expiration = expiration
                    //};

                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseContent);

                    Application.Current.Properties["Id"] = jsonResponse["id"].ToObject<int>();
                    Application.Current.Properties["Username"] = jsonResponse["name"].ToObject<string>();

                    //MessageBox.Show("Авторизация прошла успешно!");

                    MainWindow main = new MainWindow();
                    Application.Current.MainWindow.Close();
                    main.ShowDialog();
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке запроса: {ex.Message}");
            }
        }
    }
}
