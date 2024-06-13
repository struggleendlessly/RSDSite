using shared;
using shared.Enums;

using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class Services
    {
        [Parameter]
        public string? SiteName { get; set; } = null;

        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        public ServicesPageType PageType { get; set; }

        protected override void OnInitialized()
        {

            StateHasChanged();
        }

        public ServicesPageType GetPageType()
        {
            var uri = new Uri(NavigationManager.Uri);
            var pageName = uri.Segments[3];

            switch (pageName)
            {
                case StaticRoutesStrings.ItemsPageUrl:
                    return ServicesPageType.Services;
                case StaticRoutesStrings.PortfolioPageUrl:
                    return ServicesPageType.Portfolio;
                default:
                    return ServicesPageType.Services;
            }
        }
    }
}
