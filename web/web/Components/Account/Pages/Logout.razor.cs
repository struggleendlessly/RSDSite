using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;

using shared;
using web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace web.Components.Account.Pages
{
    public partial class Logout
    {
        [Inject]
        SignInManager<ApplicationUser> SignInManager { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        ClaimsPrincipal User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            User = authstate.User;

            if (SignInManager.IsSignedIn(User))
            {
                await SignInManager.SignOutAsync();
                NavigationManager.NavigateTo(StaticRoutesStrings.LogoutPageRoute, forceLoad: true);
            }
        }
    }
}
