using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace webassembly.Handlers
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) 
            : base(provider, navigation)
        {
            ConfigureHandler(
                authorizedUrls: ["http://localhost:5000"],
                scopes: ["https://myelegantpages.onmicrosoft.com/4ea2c845-d4df-4e8f-8c31-544858f64153/Api.ReadWrite"]);
        }
    }
}
