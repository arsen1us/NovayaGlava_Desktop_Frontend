using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using ClassLibForNovayaGlava_Desktop.UserModel;
using System.Net.Http.Json;
using ClassLibForNovayaGlava_Desktop;

namespace NovayaGlava_Desktop_Frontend.Utilities
{
    public class AuthService
    {
        IHttpClientFactory _httpClientFactory;
        HttpClient _client;
        CredentialHandler _credentialHandler;
        private string _apiUrl { get; } // Должно быть - "https://localhost:7245/api/";
        private static string _jwtToken; 
        private static string _refreshToken;


        public AuthService(string apiUrl, IHttpClientFactory httpClientFactory)
        {
            _credentialHandler = new CredentialHandler();

            _apiUrl = apiUrl;
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient();
        }

        public string JwtToken
        {
            get { return _jwtToken; }
            set { _jwtToken = value; }
        }

        public string RefreshToken
        {
            get { return _refreshToken; }
            set { _refreshToken = value; }
        }

        // Аутентификация пользователя
        public async Task AuthenticateAsync(string login, string password)
        {
            AuthUserModel authUserModel = new AuthUserModel(login, password);
            var response = await _client.PostAsJsonAsync(_apiUrl + "/authenticate", authUserModel);
            response.EnsureSuccessStatusCode();

            var tokenModel = await response.Content.ReadFromJsonAsync<UserTokenModel>();

            JwtToken = tokenModel.Token;
            RefreshToken = tokenModel.RefreshToken;

            // Обновить токены после успешной аутентификации
            UpdateCredential(JwtToken, RefreshToken, tokenModel.UserId);
        }

        // Обновление Refresh токена пользователя
        public async Task<string> RefreshTokenAsync()
        {
            // Получаю токены из credential, если JwtToken и RefreshToken равны null или Empty
            if(string.IsNullOrEmpty(JwtToken) || string.IsNullOrEmpty(RefreshToken))
            {
                JwtToken = _credentialHandler.GetToken("jwt");
                RefreshToken = _credentialHandler.GetToken("refresh");
            }

            TokenModel tokenModel = new TokenModel(JwtToken, RefreshToken);
            var response = await _client.PostAsJsonAsync(_apiUrl + "/token/refresh", tokenModel);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TokenModel>();

            JwtToken = result.Token;
            RefreshToken = result.RefreshToken;

            // Одновить токены в credential
            UpdateCredential(JwtToken, RefreshToken);

            return _jwtToken;
        }

        private void UpdateCredential(string jwtToken, string refreshToken)
        {
            _credentialHandler.SaveToken("jwt", "jwtToken", _refreshToken);
            _credentialHandler.SaveToken("refresh", "refreshToken", _refreshToken);
        }

        private void UpdateCredential(string jwtToken, string refreshToken, string userId)
        {
            _credentialHandler.SaveToken("jwt", "jwtToken", _refreshToken);
            _credentialHandler.SaveToken("refresh", "refreshToken", _refreshToken);
            _credentialHandler.SaveToken("userId", "userId", _refreshToken);
        }


    }
}
