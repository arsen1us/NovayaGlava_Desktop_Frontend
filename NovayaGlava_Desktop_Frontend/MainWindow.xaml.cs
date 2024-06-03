using NovayaGlava_Desktop_Frontend.Utilities;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Windows;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using Microsoft.AspNetCore.SignalR.Client;
using NovayaGlava_Desktop_Frontend.MVVM.View;
using NovayaGlava_Desktop_Frontend.MVVM.View.IdentificationPages;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;


namespace NovayaGlava_Desktop_Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AuthService _authService;
        IHttpClientFactory _clientFactory;
        CredentialHandler _credential;
        IConfiguration _configuration;

        private bool _menuIsOpen = false;

        //IServiceProvider ServiceProvider { get; set; }

        public MainWindow(AuthService authService, IHttpClientFactory clientFactory)
        {
            InitializeComponent();

            _authService = authService;
            _clientFactory = clientFactory;


            _authService = new AuthService("https://localhost:7245/api", clientFactory);
            this.Loaded += MainWindow_Loaded;
            
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e) {  }
            


        private void ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation scrollAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.1))
            };


            if (_menuIsOpen)
            {
                scrollAnimation.From = MenuPanel.ActualWidth;
                scrollAnimation.To = 0;
                _menuIsOpen = false;
                CloseButton.Visibility = Visibility.Hidden;
                OpenButton.Visibility = Visibility.Visible;
            }
            else
            {
                scrollAnimation.From = MenuPanel.ActualWidth;
                scrollAnimation.To = 200; // Desired width of the menu
                _menuIsOpen = true;
                CloseButton.Visibility = Visibility.Visible;
                OpenButton.Visibility = Visibility.Hidden;
            }

            MenuPanel.BeginAnimation(Grid.WidthProperty, scrollAnimation);
        }

    }
}