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
        public string Lang { get; set; } = string.Empty;

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

                //StateManager.UserId = null;
                //StateManager.UserEmail = null;
                //StateManager.UserSites.Clear();
            }
            else
            {
                NavigationManager.NavigateTo(StateManager.GetPageUrl(StaticRoutesStrings.EmptyRoute));
            }
        }
    }
}
