using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
    class WrapperVM
    {
        HttpClient _client;
        HubConnection _hubConnection;
        public WrapperVM() 
        {
            _client = HttpClientSingleton.Client;
            _hubConnection = ChatHubConnectionHandler.Connection;

        }

    }
}
