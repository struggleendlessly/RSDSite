using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class Pricing
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }
    }
}
