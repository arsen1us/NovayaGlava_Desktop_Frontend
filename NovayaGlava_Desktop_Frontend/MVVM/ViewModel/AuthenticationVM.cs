using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;

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

        public AuthenticationVM()
        {
            _src = "C:\\Users\\gamer\\Desktop\\WPFForTest";
            _client = HttpClientSingleton.Client;

            AuthorizeCommand = new RelayCommand(o => Authorize(Login, Password)); // , o => LoginOrPasswordIsNullOrEmpty(Login, Password)
            AuthenticationCommand = new RelayCommand(async o => await AuthenticateLocalDb());
        }

        // Отправка запроса на аутентификацию на локальный сервер
        private async Task AuthenticateLocalDb()
        {
            ChatHubConnectionHandler chatConnection = new ChatHubConnectionHandler();
            var connection = ChatHubConnectionHandler.Connection;
            _userIdHandler = new UserIdHandler();
            JwtTokenHandler jwtTokenHandler;

            UserModelForAuthentication user = new UserModelForAuthentication { Login = Login, Password = Password };

            HttpResponseMessage response = await _client.PostAsJsonAsync("https://localhost:7142/api/users/login/localdb", JsonConvert.SerializeObject(user));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string userId = await response.Content.ReadAsStringAsync();

                string token = response.Headers.GetValues("Authorization").First();
                if (string.IsNullOrEmpty(token))
                {
                    MessageBox.Show("Bad Request!");
                }
                else
                {
                    //Сохранить токен в кэше
                    jwtTokenHandler = new JwtTokenHandler();
                    jwtTokenHandler.SetToCache(token);

                    //Сохранить айди юзера в кэше
                    _userIdHandler.SetToCache(userId);
                    _userIdFileHandler = new UserIdFileHandler(userId);
                    _userIdFileHandler.SaveInFile(userId);
                    _jwtFileHandler = new JwtFileHandler(userId);
                    _jwtFileHandler.SaveInFile(token);

                    // Добавить заголовок Authorization к объекту HttpClient
                    //NovayaGlavaFrontend.Utilities.HttpClientHandler.AddAuthorizationHeader(token);
                    _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // Добавляю юзера в список юзеров подключённых к хабу


                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("Не удалось выполнить запрос");
            }

        }

        private async void Authorize(string login, string password)
        {
            UserModelForAuthentication user = new UserModelForAuthentication
            {
                Login = login,
                Password = password
            };
            string jsonUser = JsonConvert.SerializeObject(user);
            byte[] requestBuffer = new byte[jsonUser.Length];
            requestBuffer = Encoding.UTF8.GetBytes(jsonUser);

            //AuthorizationUserModel user = new AuthorizationUserModel { Login = login, Password = password };
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:7142/api/users/login/localdb");
            request.Method = "POST";
            using Stream requestStream = request.GetRequestStream();
            await requestStream.WriteAsync(requestBuffer, 0, requestBuffer.Length);

            WebResponse response = await request.GetResponseAsync();
            byte[] responseBuffer = new byte[1024];
            using Stream stream = response.GetResponseStream();
            stream.Read(responseBuffer);
            string result = Encoding.UTF8.GetString(responseBuffer);
            MessageBox.Show(result);

        }


        public void LoadKeyToXmlFile(string login = "arseniy123", string password = "bro, it's a secret!")
        {
            Data myData = new Data { Login = login, Password = password };
            XmlSerializer xmlSerializer = new XmlSerializer(myData.GetType());
            using StreamWriter streamWriter = new StreamWriter(_src);

            xmlSerializer.Serialize(streamWriter, myData);
        }

        public void GetKeyFromXmlFile()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Data));
            using StreamReader stream = new StreamReader(_src);
            Data receivedData = (Data)deserializer.Deserialize(stream);
            MessageBox.Show(receivedData.Login);
            MessageBox.Show(receivedData.Password);
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

            HttpResponseMessage response = await client.GetAsync("https://localhost:7142/api/users/testcache2");
            string responseBody = await response.Content.ReadAsStringAsync();
            MessageBox.Show(responseBody);
        }
    }

    public class Data
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
