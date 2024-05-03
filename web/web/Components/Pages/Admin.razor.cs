using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class Admin
    {
        [Parameter]
        public string? SiteName { get; set; } = null;

        [Parameter]
        public string Lang { get; set; } = string.Empty;
    }
}
