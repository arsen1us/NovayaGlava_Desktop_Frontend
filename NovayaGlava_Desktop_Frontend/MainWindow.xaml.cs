using NovayaGlava_Desktop_Frontend.Utilities;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Windows;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using Microsoft.AspNetCore.SignalR.Client;
using NovayaGlava_Desktop_Frontend.MVVM.View;


namespace NovayaGlava_Desktop_Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JwtTokenHandler _jwtTokenHandler;
        UserIdHandler _userIdHandler;
        HttpClient _client;
        HubConnection _connection;

        public MainWindow()
        {
            InitializeComponent();
            _client = HttpClientSingleton.Client;
            _jwtTokenHandler = new JwtTokenHandler();
            _userIdHandler = new UserIdHandler();
            _connection = ChatHubConnectionHandler.Connection;
            LoadView();
        }

        private void LoadView()
        {
            bool isDataAvailable = CheckDataInCache();
            if (isDataAvailable)
            {
                HttpClientSingleton.AddAuthorizationHeader(_jwtTokenHandler.GetFromCache());

                HttpResponseMessage response = Task.Run(async () => await _client.PostAsJsonAsync("https://localhost:7142/api/users/authorize", "")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //Task.Run(async () => await _connection.StartAsync()).Wait();
                    MessageBox.Show($"Успешное подключение к серверу, id подключения - {_connection.ConnectionId}");
                    MainFrame.Content = new Wrapper();
                }
                else
                {
                    MessageBox.Show("Произошла ошибка. Вы не авторизованы. Попробуйте снова");
                }
            }

            // Если нет данных в кэше, загружаю окно регистрации/авторизации
            else
            {
                MainFrame.Content = new RegistrationPage();
            }
        }

        private bool CheckDataInCache()
        {
            string token = _jwtTokenHandler.GetFromCache();
            string userId = _userIdHandler.GetFromCache();

            if (token == null || userId == null)
                return false;
            return true;
        }

    }
}