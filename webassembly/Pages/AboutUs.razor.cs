using Microsoft.AspNetCore.Components;

namespace webassembly.Pages
{
    public partial class AboutUs
    {
        [Parameter]
        public string? SiteName { get; set; } = null;

        [Parameter]
        public string Lang { get; set; } = string.Empty;
    }
}
