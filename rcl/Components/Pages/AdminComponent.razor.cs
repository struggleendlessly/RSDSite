using Microsoft.AspNetCore.Components;

using shared;

namespace rcl.Components.Pages
{
    public partial class AdminComponent
    {
        [Parameter]
        public string? SiteName { get; set; }

        public string SiteNameLower { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            SiteNameLower = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.DefaultSiteName : SiteName.ToLower();
        }
    }
}
