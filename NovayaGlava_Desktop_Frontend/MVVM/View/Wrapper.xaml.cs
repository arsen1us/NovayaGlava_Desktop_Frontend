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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovayaGlava_Desktop_Frontend.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для Wrapper.xaml
    /// </summary>
    public partial class Wrapper : UserControl
    {
        public Wrapper()
        {
            InitializeComponent();
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (PopUpMenu.ActualWidth == 1)
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = PopUpMenu.ActualWidth;
                buttonAnimation.To = 250;
                buttonAnimation.Duration = TimeSpan.FromMilliseconds(100);
                //buttonAnimation.RepeatBehavior = new RepeatBehavior(2);
                PopUpMenu.BeginAnimation(Grid.WidthProperty, buttonAnimation);

                window_button.Visibility = Visibility.Visible;
            }
            else
            {
                DoubleAnimation buttonAnimation = new DoubleAnimation();
                buttonAnimation.From = PopUpMenu.ActualWidth;
                buttonAnimation.To = 1;
                buttonAnimation.Duration = TimeSpan.FromMilliseconds(100);
                //buttonAnimation.RepeatBehavior = new RepeatBehavior(2);
                PopUpMenu.BeginAnimation(Grid.WidthProperty, buttonAnimation);

                window_button.Visibility = Visibility.Hidden;
            }
        }
    }
}
