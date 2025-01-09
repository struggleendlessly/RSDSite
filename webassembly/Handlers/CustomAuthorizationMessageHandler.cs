using System.Net.Http.Headers;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace webassembly.Handlers
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider _provider;
        private readonly NavigationManager _navigation;

        public CustomAuthorizationMessageHandler(
            IAccessTokenProvider provider, 
            NavigationManager navigation)
        {
            _provider = provider;
            _navigation = navigation;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenResult = await _provider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
