using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace PokerTime.App
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        private readonly IConfiguration _configuration;

        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager, IConfiguration configuration) : base(provider, navigationManager)
        {
            _configuration = configuration;
            ConfigureHandler(
                authorizedUrls: new[] { "https://pokertimeapi.azurewebsites.net" },
                scopes: new[] { "api-access" });
            
        }
    }
}
