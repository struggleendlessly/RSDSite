using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class Service
    {
        [Parameter]
        public string? SiteName { get; set; } = null;

        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Parameter]
        public string UrlKey { get; set; } = string.Empty;
    }
}
