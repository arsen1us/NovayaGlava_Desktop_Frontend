using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    public class ProfileSettingsVM
    {
        public ObservableCollection<string> ProfileSettingsMenu { get; set; }
        HttpClient _client;

        public ProfileSettingsVM()
        {
            _client = HttpClientSingleton.Client;
            ProfileSettingsMenu = new ObservableCollection<string>
            {
                "Личная информация",
                "Профиль",
                "Контакты",

            };
        }
    }
}
