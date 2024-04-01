using shared;
using shared.Models;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Shared
{
    public partial class FeaturedCaseStudies
    {
        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        [Parameter]
        public string SiteName { get; set; } = string.Empty;

        public string SiteNameLower { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            SiteNameLower = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.DefaultSiteName : SiteName.ToLower();
        }
    }
}
