using shared;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Pages
{
    public partial class SubscriptionErrorComponent
    {
        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }
    }
}
