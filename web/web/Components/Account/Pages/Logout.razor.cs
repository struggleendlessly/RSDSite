using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Data.Entities;

namespace web.Components.Account.Pages
{
    public partial class Logout
    {
        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Inject]
        SignInManager<User> SignInManager { get; set; }

        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (SignInManager.IsSignedIn(authState.User))
            {
                await SignInManager.SignOutAsync();
                NavigationManager.Refresh(forceReload: true);
            }
        }
    }
}
