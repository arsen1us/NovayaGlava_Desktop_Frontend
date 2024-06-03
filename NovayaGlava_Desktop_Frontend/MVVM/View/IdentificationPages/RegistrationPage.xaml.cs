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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovayaGlava_Desktop_Frontend.MVVM.View.IdentificationPages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        IdentificationWindow _identificationWindow;
        IdentificationWindowVM _identificationVM;

        public RegistrationPage(IdentificationWindow identificationWindow, IdentificationWindowVM identificationVM)
        {
            InitializeComponent();
            _identificationWindow = identificationWindow;
            _identificationVM = identificationVM;
            DataContext = _identificationVM;
        }
        public void ChangeToAuthenticationPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthenticationPage(_identificationWindow, _identificationVM));
        }
    }
}
