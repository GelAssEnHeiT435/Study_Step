﻿using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Commands;
using Study_Step.Interfaces;
using Study_Step.Models;
using Study_Step.Models.DTO;
using Study_Step.Pages;
using Study_Step.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Study_Step.ViewModels
{
    public class AuthViewModel: INotifyPropertyChanged
    {
        #region Properties
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        public string? Username
        {
            get => _username;
            set
            {
                if (_username != value) {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        public string? Email
        {
            get => _email;
            set
            {
                if (_email != value) {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                if (_password != value) {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private Page _currentPage;
        private string? _username;
        private string? _email;
        private string? _password;

        #endregion

        private readonly SignalRService _signalRService;
        private readonly ITokenStorage _tokenStorage;
        private readonly AuthService _authService;
        private readonly UserSessionService _userSession;
        private readonly DtoConverterService _dtoConverter;
        private readonly HttpClient _httpClient = new HttpClient();

        public AuthViewModel(SignalRService signalRService, ITokenStorage tokenStorage,
                             AuthService authService, UserSessionService userSession, DtoConverterService dtoConverterService)
        {
            _signalRService = signalRService;
            _tokenStorage = tokenStorage;
            _authService = authService;
            _userSession = userSession;
            _dtoConverter = dtoConverterService;
        }

        #region Commands

        public ICommand NavigateToRegisterPageCommand => _navigateToRegisterPageCommand ??= new RelayCommand(parameter =>
            NavigateToRegisterPage());

        public ICommand NavigateToSignInPageCommand => _navigateToSignInPageCommand ??= new RelayCommand(parameter =>
            NavigateToSignInPage());

        public ICommand LoginCommand => _loginCommand ??= new RelayCommand(parameter =>
            UserLogin());

        public ICommand RegisterCommand => _registerCommand ??= new RelayCommand(parameter => 
            UserRegister());


        private ICommand _navigateToRegisterPageCommand;
        private ICommand _navigateToSignInPageCommand;
        private ICommand _loginCommand;
        private ICommand _registerCommand;
        #endregion

        #region Logic
        private void NavigateToRegisterPage() =>
            CurrentPage = App.ServiceProvider.GetRequiredService<RegisterPage>();

        private void NavigateToSignInPage() =>
            CurrentPage = App.ServiceProvider.GetRequiredService<SignInPage>();

        private async void UserLogin()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) { return; }

            AuthUser user = new AuthUser
            {
                Email = Email,
                Password = Password
            };

            // Преобразуем объект пользователя в JSON строку и формируем HTTP-запрос
            string json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try {
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:5000/login", content); // Отправляем запрос на сервер

                if (response.IsSuccessStatusCode) { // Проверяем статус ответа
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject? jsonResponse = JsonConvert.DeserializeObject<JObject>(responseContent);

                    UserDTO? currentUser = jsonResponse["user_object"].ToObject<UserDTO>();
                    _userSession.CurrentUser = _dtoConverter.GetUser(currentUser);
                    string? AccessToken = jsonResponse["access_token"].ToObject<string>();
                    string? RefreshToken = jsonResponse["refresh_token"].ToObject<string>();

                    _authService.AccessToken = AccessToken;
                    _tokenStorage.SaveRefreshToken(RefreshToken);
                    await _signalRService.ConnectAsync(AccessToken);
                    

                    MainWindow main = App.ServiceProvider.GetRequiredService<MainWindow>();
                    Application.Current.MainWindow = main;
                    Application.Current.Windows.OfType<AuthWindow>().FirstOrDefault()?.Close();
                    main.Show();
                }
                else {
                    string error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Ошибка: {error}");
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Ошибка при отправке запроса: {ex.Message}");
            }
        }

        private async void UserRegister()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) { return; }

            AuthUser user = new AuthUser
            {
                Name = Username,
                Email = Email,
                Password = Password
            };

            // Преобразуем объект пользователя в JSON строку и формируем HTTP-запрос
            string json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Debug.WriteLine(json);

            try {
                HttpResponseMessage response = await _httpClient.PostAsync("http://localhost:5000/register", content); // Отправляем запрос на сервер

                if (response.IsSuccessStatusCode) { // Проверяем статус ответа
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject? jsonResponse = JsonConvert.DeserializeObject<JObject>(responseContent);
                    

                    UserDTO? currentUser = jsonResponse["user_object"].ToObject<UserDTO>();
                    _userSession.CurrentUser = _dtoConverter.GetUser(currentUser);
                    string? AccessToken = jsonResponse["access_token"].ToObject<string>();
                    string? RefreshToken = jsonResponse["refresh_token"].ToObject<string>();

                    await _signalRService.ConnectAsync(AccessToken);
                    await _signalRService.NotifyNewUser(_userSession.GetCurrentUserDTO());
                    await _signalRService.DisconnectAsync();

                    NavigateToSignInPage();
                }
                else {
                    string error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Ошибка: {error}");
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Ошибка при отправке запроса: {ex.Message}");
            }
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
