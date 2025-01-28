using shared.Interfaces;
using shared.Interfaces.Api;

using Microsoft.AspNetCore.Components;

namespace webassembly.Pages
{
    public partial class Authentication
    {
        [Parameter] public string? Action { get; set; }

        [Inject] IApiUserService ApiUserService { get; set; } = default!;

        [Inject] IApiWebsiteService ApiWebsiteService { get; set; } = default!;

        [Inject] IStateManager StateManager { get; set; } = default!;

        public async Task OnLogInSucceeded()
        {
            var user = await ApiUserService.GetOrCreateAsync();
            var websites = await ApiWebsiteService.GetAllAsync();

            StateManager.User = user;
            StateManager.UserWebsites = websites;
        }
    }
}
