using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NovayaGlava_Desktop_Frontend.MVVM.ViewModel
{
    class PasswordRecoveryVM
    {
        public string Email { get; set; }
        HttpClient _client;
        public PasswordRecoveryVM()
        {
            _client = new HttpClient();
        }

    }
}
