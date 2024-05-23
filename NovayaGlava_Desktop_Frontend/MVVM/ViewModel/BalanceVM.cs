using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class BalanceVM : ViewModelBase
    {
        private string _balance;
        public string Balance
        {
            get { return _balance; }
            set
            {
                _balance = value; 
                OnPropertyChanged();
            }
        }
        public BalanceVM() 
        {
            _balance = "1000";
        }
    }
}
