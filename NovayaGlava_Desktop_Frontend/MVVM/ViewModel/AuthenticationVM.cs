using ClassLibForNovayaGlava_Desktop.UserModel;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class AuthenticationVM
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public string KeyWord { get; set; }

        public string _src { get; set; }

        public RelayCommand AuthorizeCommand { get; set; }
        public RelayCommand AuthenticationCommand { get; set; }

        

        UserIdHandler _userIdHandler;
        HttpClient _client;
        HubConnection _hubConnection;
        UserIdFileHandler _userIdFileHandler;
        JwtFileHandler _jwtFileHandler;
        CredentialHandler _credential;

        AuthService _authService;
        public IServiceProvider ServiceProvider { get; private set; }

        public AuthenticationVM()
        {
            _client = HttpClientSingleton.Client;
            _credential = new CredentialHandler();

            AuthenticationCommand = new RelayCommand(async o => await AuthenticateAsync());

            ServiceProvider = ServiceProviderContainer.ServiceProvider;
            _authService = new AuthService("https://localhost:7245/api", ServiceProvider.GetRequiredService<IHttpClientFactory>());
        }

        // Аутентификация 
        private async Task AuthenticateAsync()
        {
            ChatHubConnectionHandler chatConnection = new ChatHubConnectionHandler();
            var connection = ChatHubConnectionHandler.Connection;

            await _authService.AuthenticateAsync(Login, Password);

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private bool LoginOrPasswordIsNullOrEmpty(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;
            return true;
        }

        //Отправить токен на сервер для добавления в cache
        private async Task SendTokenToServerCache(string jwtToken)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"{jwtToken}");

            HttpResponseMessage response = await client.GetAsync("https://localhost:7245/api/users/testcache2");
            string responseBody = await response.Content.ReadAsStringAsync();
            MessageBox.Show(responseBody);
        }
    }
}



