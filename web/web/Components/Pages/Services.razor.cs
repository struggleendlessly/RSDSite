using shared;
using shared.Enums;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

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

        private ServicesPageType theme;
        protected override void OnInitialized()
        {

            StateHasChanged();
        }

        public ServicesPageType GetPageType()
        {
            var uri = new Uri(NavigationManager.Uri);
            var pageName = uri.Segments[3];
            var res = ServicesPageType.Services;
            switch (pageName)
            {
                case StaticRoutesStrings.ItemsPageUrl:
                    res=  ServicesPageType.Services;
                    break;
                case StaticRoutesStrings.PortfolioPageUrl:
                    res=  ServicesPageType.Portfolio;
                    break;
                default:
                    res =  ServicesPageType.Services;
                    break;
            }
            theme = res;
            return res;
        }
    }
}
