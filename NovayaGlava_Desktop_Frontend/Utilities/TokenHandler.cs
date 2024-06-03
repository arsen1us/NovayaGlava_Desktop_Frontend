using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovayaGlava_Desktop_Frontend.Utilities
{
    public class TokenHandler : DelegatingHandler
    {
        AuthService _authService;

        public TokenHandler(AuthService authService)
        {
            _authService = authService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(_authService.JwtToken);

            var response = await base.SendAsync(request, cancellationToken);

            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                string newToken = await _authService.RefreshTokenAsync();
                request.Headers.Authorization = new AuthenticationHeaderValue(newToken);
                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }
    }
}
