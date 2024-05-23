using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;
using System.Net.Http;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class NavigationVM : ViewModelBase
    {
        HubConnection _connection = ChatHubConnectionHandler.Connection;
        HttpClient _client;

        public RelayCommand OnClickMenuButton { get; set; }
        public RelayCommand ConnectToLocalServerCommand { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand BalanceCommand { get; set; }
        public ICommand ChatCommand { get; set; }
        public ICommand CompaniesCommand { get; set; }
        public ICommand ContentCommand { get; set; }
        public ICommand FavoriteCommand { get; set; }
        public ICommand FriendsCommand { get; set; }
        public ICommand LevelCommand { get; set; }
        public ICommand NewsCommand { get; set; }
        public ICommand QuitCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        public ICommand HomeCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand ProfileSettingsCommand { get; set; }

        public ICommand RegistrationCommand { get; set; }
        public ICommand AuthorizationCommand { get; set; }

        // Обёртка для отображения регистрации и авторизации
        public ICommand WrapperCommand { get; set; }

        // Обёртка для перехода от списка пользователей к профилю юзера
        public ICommand FriendsWrapperCommand { get; set; }


        public void MenuButtonClick() { }

        private void Balance(object obj) => CurrentView = new BalanceVM();
        private void Chat(object obj) => CurrentView = new ChatVM();
        private void Companies(object obj) => CurrentView = new CompaniesVM();
        private void Content(object obj) => CurrentView = new ContentVM();
        private void Favorite(object obj) => CurrentView = new FavoriteVM();
        private void Friends(object obj) => CurrentView = new FriendsVM();
        private void Level(object obj) => CurrentView = new LevelVM();
        private void News(object obj) => CurrentView = new NewsVM();
        private void Quit(object obj) => CurrentView = new QuitVM();
        private void Settings(object obj) => CurrentView = new SettingsVM();

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Profile(object obj) => CurrentView = new ProfileVM();
        private void ProfileSettings(object obj) => CurrentView = new ProfileSettingsVM();

        private void Registration(object obj) => CurrentView = new RegistrationVM();
        private void Authorization(object obj) => CurrentView = new AuthenticationVM();

        private void Wrapper(object obj) => CurrentView = new WrapperVM();
        private void FriendsWrapper(object obj) => CurrentView = new FriendsWrapperVM();

        public NavigationVM()
        {
            _client = HttpClientSingleton.Client;
            OnClickMenuButton = new RelayCommand(o => MenuButtonClick());

            BalanceCommand = new RelayCommand(Balance);
            ChatCommand = new RelayCommand(Chat);
            CompaniesCommand = new RelayCommand(Companies);
            ContentCommand = new RelayCommand(Content);
            FavoriteCommand = new RelayCommand(Favorite);
            FriendsCommand = new RelayCommand(Friends);
            LevelCommand = new RelayCommand(Level);
            NewsCommand = new RelayCommand(News);
            QuitCommand = new RelayCommand(Quit);
            SettingsCommand = new RelayCommand(Settings);

            HomeCommand = new RelayCommand(Home);
            ProfileCommand = new RelayCommand(Profile);
            ProfileSettingsCommand = new RelayCommand(ProfileSettings);

            RegistrationCommand = new RelayCommand(Registration);
            AuthorizationCommand = new RelayCommand(Authorization);

            WrapperCommand = new RelayCommand(Wrapper);
            FriendsWrapperCommand = new RelayCommand(FriendsWrapper);

            CurrentView = new HomeVM();

            //ConnectToLocalServerCommand = new RelayCommand(async o => await ConnectToLocalServer());

            // Сразу при заходе на страницу вызывает метод
            // Затычка
            //ConnectToLocalServerCommand.Execute("");
        }

        private async Task ConnectToLocalServer()
        {
            _connection = ChatHubConnectionHandler.Connection;
            await _connection.StartAsync();
            //MessageBox.Show(_connection.ConnectionId);
        }
    }
}

