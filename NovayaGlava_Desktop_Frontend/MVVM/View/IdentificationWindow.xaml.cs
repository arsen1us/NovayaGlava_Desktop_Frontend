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
using System.Windows.Shapes;
using NovayaGlava_Desktop_Frontend.MVVM.View.IdentificationPages;
using NovayaGlava_Desktop_Frontend.Utilities;
using NovayaGlava_Desktop_Frontend.MVVM.ViewModel;

namespace NovayaGlava_Desktop_Frontend.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для IdentificationWindow.xaml
    /// </summary>
    public partial class IdentificationWindow : Window, ICloseble
    {
        public IdentificationWindow()
        {
            InitializeComponent();

            var viewModel = new IdentificationWindowVM(this);
            DataContext = viewModel;

            IdentificationFrame.Navigate(new RegistrationPage(this, viewModel));
        }

        public void CloseWindow()
        {
            this.Close();
        }
    }
}
