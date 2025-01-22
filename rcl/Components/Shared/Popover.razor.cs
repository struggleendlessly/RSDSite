using shared;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Shared
{
    public partial class Popover
    {
        [Parameter]
        public PageModel PopoversModel { get; set; }

        [Parameter]
        public string PopoverKey { get; set; }

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; } = default!;

        [Inject] IStateManager StateManager { get; set; } = default!;

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }
    }
}
