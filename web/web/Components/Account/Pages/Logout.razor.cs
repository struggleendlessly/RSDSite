using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;

using shared;
using web.Data;

namespace web.Components.Account.Pages
{
    public partial class Logout
    {
        [Inject]
        SignInManager<ApplicationUser> SignInManager { get; set; }

        [Inject]
        IdentityRedirectManager RedirectManager { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SignInManager.SignOutAsync();
            //RedirectManager.RedirectTo(StaticRoutesStrings.MainPageRoute);
            NavigationManager.NavigateTo(StaticRoutesStrings.MainPageRoute, forceLoad: true);
            StateHasChanged();
        }
    }
}
