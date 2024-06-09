using shared;
using shared.Models;
using shared.Interfaces;

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
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }
    }
}
