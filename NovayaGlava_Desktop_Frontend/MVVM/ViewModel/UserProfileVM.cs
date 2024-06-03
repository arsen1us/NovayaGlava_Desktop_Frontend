using NovayaGlava_Desktop_Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NovayaGlava_Desktop_Frontend.FileHandlers;
using NovayaGlava_Desktop_Frontend.CacheHandlers;
using NovayaGlava_Desktop_Frontend.Utilities;
using ClassLibForNovayaGlava_Desktop;
using System.Windows.Controls;
using NovayaGlava_Desktop_Frontend.MVVM.View;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    internal class UserProfileVM
    {
        HttpClient _client;
        public UserProfileVM(UserControl view, User user)
        {
            _client = HttpClientSingleton.Client;
        }

        private async Task GetUserByIdLocalDb(string userId)
        {
            
        }
    }
}
