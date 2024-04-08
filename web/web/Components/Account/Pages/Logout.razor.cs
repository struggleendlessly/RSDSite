using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Interfaces;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class Logout
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }

        [Inject]
        SignInManager<ApplicationUser> SignInManager { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (SignInManager.IsSignedIn(authstate.User))
            {
                await SignInManager.SignOutAsync();           
            }
            else
            {
                NavigationManager.NavigateTo(GetPageUrl(StaticRoutesStrings.EmptyRoute));
            }
        }

        public string GetPageUrl(string url)
        {
            return $"{StateManager.SiteName}/{StateManager.Lang}/{url}";
        }
    }
}
