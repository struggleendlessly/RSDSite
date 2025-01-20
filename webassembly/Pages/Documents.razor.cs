using Microsoft.AspNetCore.Components;

namespace webassembly.Pages
{
    public partial class Documents
    {
        [Parameter]
        public string? SiteName { get; set; } = null;

        [Parameter]
        public string Lang { get; set; } = string.Empty;
    }
}
