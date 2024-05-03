using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class Home
    {
        [Parameter]
        public string? SiteName { get; set; } = null;

        [Parameter]
        public string Lang { get; set; } = string.Empty;
    }
}
