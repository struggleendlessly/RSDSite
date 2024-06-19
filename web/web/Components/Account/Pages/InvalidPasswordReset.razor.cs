using shared;
using shared.Models;
using shared.Interfaces;

using Microsoft.AspNetCore.Components;

namespace web.Components.Account.Pages
{
    public partial class InvalidPasswordReset
    {
        [Parameter]
        public string Lang { get; set; } = string.Empty;

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }
    }
}
