using Microsoft.AspNetCore.Components;
using shared.Interfaces;

namespace web.Components.Account.Pages
{
    public partial class ResetPasswordConfirmation
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        public string GetPageUrl(string url)
        {
            return $"{StateManager.SiteName}/{StateManager.Lang}/{url}";
        }
    }
}
