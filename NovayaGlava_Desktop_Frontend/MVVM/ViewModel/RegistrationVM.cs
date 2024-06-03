using ClassLibForNovayaGlava_Desktop;
using ClassLibForNovayaGlava_Desktop.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using System.Security.RightsManagement;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Navigation;
using Newtonsoft.Json.Bson;
using Microsoft.Extensions.Caching.Distributed;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;
using Microsoft.Extensions.DependencyInjection;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class RegistrationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string KeyWord { get; set; }

        public RelayCommand RegistrationCommand { get; set; }
        public RelayCommand HomeCommand { get; set; } // Команда для переключения на вьюшку главной страницы;
        public RelayCommand AuthorizationCommand { get; set; } // Команда для переключения на вьюшку с авторизацией;

        public RelayCommand OpenAuthenticationPageCommand { get; set; }

        IDistributedCache _cache;
        UserIdHandler _userIdHandler;
        HttpClient _client;
        public IServiceProvider ServiceProvider { get; private set; }

        public RegistrationVM()
        {
            _client = HttpClientSingleton.Client;
            RegistrationCommand = new RelayCommand(async o => await RegisterLocalDb());

            HomeCommand = new RelayCommand(Home); // переключение на вьюшку с главной страницей
            AuthorizationCommand = new RelayCommand(Authorization); // переключение на вьюшку с авторизацией
        }

        private void Home(object obj) => CurrentView = new NavigationVM();
        private void Authorization(object obj) => CurrentView = new AuthenticationVM();
        


        // Отправка запроса на регистрацию на сервер NovayaGlava-Backend
        private async Task Register()
        {
            var bodyData = new { nickname = Login, password = Password };

            HttpResponseMessage response = await _client.PostAsJsonAsync("http://localhost:1337/api/user/register", bodyData);

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show(await response.Content.ReadAsStringAsync());

            //Если юзер успешно зарегестрировался - открывается главное окно

            string responseString = await response.Content.ReadAsStringAsync();
            if (responseString == "{\"message\":\"Successfully registered\",\"status\":\"OK\"}")
            {
                //WrapperVM wrapperVM = new WrapperVM();
                //Wrapper wrapperView = new Wrapper();
                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
        }

        // Отправка запроса на регистрацию на локальный сервер
        private async Task RegisterLocalDb()
        {
            _userIdHandler = new UserIdHandler();
            JwtTokenHandler jwtTokenHandler;

            UserModel user = new UserModel
            {
                _id = Guid.NewGuid().ToString(),
                NickName = Login,
                Email = Email,
                Password = Password,
            };
            string jsonUser = JsonConvert.SerializeObject(user);
            HttpResponseMessage response = await _client.PostAsJsonAsync("https://localhost:7245/api/users/registration/localdb", jsonUser);

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Не удалось зарегистрировать нового пользователя");
            else
            {
                //Сохранить токен в кэше
                string jwtToken = response.Headers.GetValues("Authorization").First();
                jwtTokenHandler = new JwtTokenHandler();
                jwtTokenHandler.SetToCache(jwtToken);

                //Сохранить айди юзера в кэше
                _userIdHandler.SetToCache(user._id);

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
        }

        private bool LoginOrEmailOrPasswordIsNullOrEmpty()
        {
            if (Login == null || Login == string.Empty
                || Email == null || Email == string.Empty
                || Password == null || Password == string.Empty)
                return false;
            return true;
        }

        private void OpenAuthenticationPage()
        {
            
        }

    }
}
