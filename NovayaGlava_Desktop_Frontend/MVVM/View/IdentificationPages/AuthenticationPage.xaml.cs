using NovayaGlava_Desktop_Frontend.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovayaGlava_Desktop_Frontend.MVVM.View.IdentificationPages
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationPage.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {
        IdentificationWindow _identificationWindow;
        IdentificationWindowVM _identificationVM;

        public AuthenticationPage(IdentificationWindow identificationWindow, IdentificationWindowVM identificationVM)
        {
            InitializeComponent();
            StartAnimation();
            _identificationWindow = identificationWindow;
            _identificationVM = identificationVM;
            this.DataContext = _identificationVM;
            
        }

        // Открыть окно регистрации
        public void ChangeToRegistrationPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(_identificationWindow, _identificationVM));
        }

        // Открыть окно восстановления пароля
        public void ChangeToPasswordRecoveryPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PasswordRecoveryPage(_identificationWindow, _identificationVM));
        }

        private void StartAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                RepeatBehavior = RepeatBehavior.Forever
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }
    }
}
