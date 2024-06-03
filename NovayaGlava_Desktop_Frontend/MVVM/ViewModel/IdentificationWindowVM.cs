using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.MVVM.View;
using NovayaGlava_Desktop_Frontend.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    public class IdentificationWindowVM : INotifyPropertyChanged
    {
        ICloseble _window;

        private string _login;
        private string _email;
        private string _password;

        public ICommand AuthenticationCommand { get; set; }

        HubConnection _hubConnection;

        AuthService _authService;

        public IServiceProvider ServiceProvider { get; private set; }

        //public IdentificationWindowVM()
        //{
        //    AuthenticationCommand = new RelayCommand(async o => await AuthenticateAsync());

        //    ServiceProvider = ServiceProviderContainer.ServiceProvider;
        //    _authService = new AuthService("https://localhost:7245/api", ServiceProvider.GetRequiredService<IHttpClientFactory>());
        //}

        public IdentificationWindowVM(ICloseble window)
        {
            _window = window;
            AuthenticationCommand = new RelayCommand(async o => await AuthenticateAsync());

            ServiceProvider = ServiceProviderContainer.ServiceProvider;
            _authService = new AuthService("https://localhost:7245/api", ServiceProvider.GetRequiredService<IHttpClientFactory>());
        }

        public string Login { get => _login; set { _login = value; OnPropertyChanged(nameof(Login)); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }

        private async Task AuthenticateAsync()
        {
            ChatHubConnectionHandler chatConnection = new ChatHubConnectionHandler();
            var connection = ChatHubConnectionHandler.Connection;

            await _authService.AuthenticateAsync(Login, Password);

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            CloseWindow();
            mainWindow.Show();
        }

        public void CloseWindow()
        {
            _window.CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
