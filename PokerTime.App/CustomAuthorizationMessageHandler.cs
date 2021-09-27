using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace PokerTime.App
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager) : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://pokertimeapi.azurewebsites.net" },
                scopes: new[] { "api-access" });
        }
    }
}
