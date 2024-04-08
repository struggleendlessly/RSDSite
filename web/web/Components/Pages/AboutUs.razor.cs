using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class AboutUs
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }
    }
}
