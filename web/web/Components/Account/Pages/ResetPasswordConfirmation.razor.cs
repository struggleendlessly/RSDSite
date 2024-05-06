using Microsoft.AspNetCore.Components;
using shared.Interfaces;

namespace web.Components.Account.Pages
{
    public partial class ResetPasswordConfirmation
    {
        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Inject]
        IStateManager StateManager { get; set; }
    }
}
