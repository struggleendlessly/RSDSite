using shared;
using shared.Models;
using shared.Helpers;
using shared.Interfaces;
using shared.Interfaces.Api;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Shared
{
    public partial class FeaturedCaseStudies
    {
        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        [Parameter]
        public PageModel? PopoversModel { get; set; } = null;

        [Parameter]
        public string? PopoverKey { get; set; } = null;

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        private int ItemsCount { get; set; } = 4;

        protected override async Task OnInitializedAsync()
        {
            var serviceItems = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.ServicesPageServicesListDataJsonFilePath);
            ServiceItems = VisibilityHelpers.GetVisibleServiceItems(serviceItems, ItemsCount);
        }
    }
}
