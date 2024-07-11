using shared;
using shared.Models;
using shared.Interfaces;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Shared
{
    public partial class SEO
    {
        [Parameter]
        public PageModel Model { get; set; } = new PageModel();

        [Parameter]
        public Func<PageModel, Task> FuncSave { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }
    }
}
