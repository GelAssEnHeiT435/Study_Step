using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Study_Step.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStorage _tokenStorage;
        private readonly SignalRService _signalRService;
        public string? AccessToken
        {
            get => _accessToken;
            set => _accessToken = value;
        }
        private string _accessToken;

        public AuthService(ITokenStorage tokenStorage, SignalRService signalRService)
        {
            _httpClient = new HttpClient();
            _tokenStorage = tokenStorage;
            _signalRService = signalRService;
        }

        public async Task<bool> TryAutoLoginAsync()
        {
            string? refreshToken = _tokenStorage.LoadRefreshToken();
            if (string.IsNullOrEmpty(refreshToken)) return false;

            string json = JsonConvert.SerializeObject(refreshToken);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5000/refresh", content);

            if (!response.IsSuccessStatusCode) return false;

            var tokens = await response.Content.ReadAsStringAsync();
            JObject? jsonResponse = JsonConvert.DeserializeObject<JObject>(tokens);

            Application.Current.Properties["Id"] = jsonResponse["id"].ToObject<int>();
            Application.Current.Properties["Username"] = jsonResponse["name"].ToObject<string>();
            AccessToken = jsonResponse["access_token"].ToObject<string>();
            string RefreshToken = jsonResponse["refresh_token"].ToObject<string>();

            await _signalRService.ConnectAsync(AccessToken);

            _tokenStorage.SaveRefreshToken(RefreshToken);
            return true;
        }

        public async Task LogoutAsync()
        {
            string? refreshToken = _tokenStorage.LoadRefreshToken();
            if (string.IsNullOrEmpty(refreshToken)) return;

            try {
                string json = JsonConvert.SerializeObject(refreshToken);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("http://localhost:5000/logout", content);
            }
            catch (Exception ex) {
                Debug.WriteLine($"Ошибка при выходе: {ex.Message}");
            }
            finally {
                _tokenStorage.DeleteRefreshToken();
                AccessToken = null;

                await _signalRService.DisconnectAsync();
            }
        }
    }
}
