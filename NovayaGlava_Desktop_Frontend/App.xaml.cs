using NovayaGlava_Desktop_Frontend.Utilities;
using System.CodeDom;
using System.Configuration;
using System.Data;
using System.Windows;
using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Http;
using System.Net.Http.Headers;
using NovayaGlava_Desktop_Frontend.MVVM.View;
using Microsoft.Extensions.DependencyInjection;
using NovayaGlava_Desktop_Frontend.MVVM.ViewModel;

namespace NovayaGlava_Desktop_Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        AuthService _authService;
        HttpClient _client;
        CredentialHandler _credential;

        public IServiceProvider ServiceProvider { get; set; }

        public App()
        {
            _client = HttpClientSingleton.Client;
            _credential = new CredentialHandler();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Конфигурация сервисов
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            ServiceProviderContainer.ServiceProvider = ServiceProvider;

            _authService = new AuthService("https://localhost:7245/api", ServiceProvider.GetRequiredService<IHttpClientFactory>());

            await AuthenticationAsync();

        }

        private async Task AuthenticationAsync()
        {
            //_credential.DeleteToken("jwt");
            string jwtToken = _credential.GetToken("jwt");
            if (jwtToken is null || jwtToken == "")
            {
                IdentificationWindow identificationWindow = new IdentificationWindow();
                identificationWindow.Show();
            }
            else
            {
                _client.DefaultRequestHeaders.Add("Authorization","Bearer " + jwtToken);
                var response = await _client.GetAsync("https://localhost:7245/api/token/check-authorization");
                //response.EnsureSuccessStatusCode();

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    IdentificationWindow identificationWindow = new IdentificationWindow();
                    identificationWindow.Show();
                }

                else if (response.StatusCode == HttpStatusCode.OK)
                {
                    var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                }
                else
                {
                    throw new Exception($"Неожиданный ответ от сервера. Status code - {response.StatusCode}");
                }
            }
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection
                .AddSingleton<AuthService>(provider => new AuthService("https://localhost:7245/", provider.GetRequiredService<IHttpClientFactory>()))
                .AddHttpClient("ApiClient")
                .AddHttpMessageHandler(provider => new TokenHandler(provider.GetRequiredService<AuthService>()))
                .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://localhost:7245/"));

            // Регистрация MainWindow
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddTransient<ChatVM>();
        }
    }

}
