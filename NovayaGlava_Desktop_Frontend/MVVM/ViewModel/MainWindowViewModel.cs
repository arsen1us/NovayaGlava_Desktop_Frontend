using NovayaGlava_Desktop_Frontend.Utilities;
using NovayaGlava_Desktop_Frontend.MVVM.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowChatViewCommand { get; }
        public ICommand ShowNewsViewCommand { get; }
        public ICommand ShowFriendsViewCommand { get; }

        public MainWindowViewModel()
        {
            ShowChatViewCommand = new RelayCommand(o => CurrentView = new Chat());
            ShowNewsViewCommand = new RelayCommand(o => CurrentView = new NewsWindow());
            ShowFriendsViewCommand = new RelayCommand(o => CurrentView = new FriendsUserControl());
            CurrentView = new NewsWindow();
        }

        //private void ShowUserProfile(object user)
        //{
        //    if (user is User)
        //        CurrentView = new UserProfileVM(this, user);
        //}



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
