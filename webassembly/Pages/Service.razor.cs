using shared;
using shared.Enums;

using Microsoft.AspNetCore.Components;

namespace webassembly.Pages
{
    public partial class Service
    {
        [Parameter]
        public string? SiteName { get; set; } = null;

        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Parameter]
        public string UrlKey { get; set; } = string.Empty;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        public ServicesPageType PageType { get; set; }

        protected override void OnInitialized()
        {
            var uri = new Uri(NavigationManager.Uri);
            var pageName = uri.Segments[3].Replace("/", string.Empty);

            switch (pageName)
            {
                case StaticRoutesStrings.ItemsPageUrl:
                    PageType = ServicesPageType.Services;
                    break;
                case StaticRoutesStrings.PortfolioPageUrl:
                    PageType = ServicesPageType.Portfolio;
                    break;
                case StaticRoutesStrings.DocumentsPageUrl:
                    PageType = ServicesPageType.Documents;
                    break;
                default:
                    PageType = ServicesPageType.Services;
                    break;
            }
        }
    }
}
