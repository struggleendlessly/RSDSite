using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class SubscriptionError
    {
        [Parameter]
        public string SiteName { get; set; } = string.Empty;

        [Parameter]
        public string Lang { get; set; } = string.Empty;
    }
}
