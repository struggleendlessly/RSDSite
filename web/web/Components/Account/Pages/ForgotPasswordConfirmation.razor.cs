using Microsoft.AspNetCore.Components;

namespace web.Components.Account.Pages
{
    public partial class ForgotPasswordConfirmation
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }
    }
}
