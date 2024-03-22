using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using web.Data;

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

        protected override async Task OnInitializedAsync()
        {
            var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (SignInManager.IsSignedIn(authstate.User))
            {
                await SignInManager.SignOutAsync();           
            }
            else
            {
                NavigationManager.NavigateTo(StaticRoutesStrings.MainPageRoute);
            }
        }
    }
}
