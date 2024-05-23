using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NovayaGlava_Desktop_Frontend
{
    public class HttpClientSingleton
    {
        private static HttpClient _client;

        private static readonly object _lockObject = new object();

        private HttpClientSingleton() { }

        public static HttpClient Client
        {
            get
            {
                lock (_lockObject)
                {
                    if (_client != null)
                        return _client;
                    _client = new HttpClient();
                    return _client;
                }
            }
        }

        public static void AddAuthorizationHeader(string jwtToken)
        {
            // Добавить сравнение с токеном, который уже лежит в кэше

            if (_client == null)
                throw new Exception(" Не был инициализирован объект HttpClient. Для начала инициализируйте его");
            if (_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                string token = _client.DefaultRequestHeaders.GetValues("Authorization").First();
                if (token != jwtToken)
                    UpdateToken(jwtToken);
            }
            else
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            }
        }

        public static void UpdateToken(string jwtToken)
        {
            _client.DefaultRequestHeaders.Remove("Authorization");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
        }
    }
}
