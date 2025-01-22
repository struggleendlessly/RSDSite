using shared;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Shared
{
    public partial class VisibilityEditor
    {
        [Parameter]
        public string EditorId { get; set; } = string.Empty;

        [Parameter]
        public Func<PageModel, Task> FuncSave { get; set; }

        [Parameter]
        public PageModel Model { get; set; } = new PageModel();

        [Parameter]
        public string Key { get; set; } = string.Empty;

        [Inject] IApiPageDataService ApiPageDataService { get; set; } = default!;

        [Inject] IStateManager StateManager { get; set; } = default!;

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {     
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }

        private async Task CheckboxChangedAsync(ChangeEventArgs e)
        {
            var key = Key + StaticStrings.IsVisibleKeyEnding;
            var value = e.Value!.ToString()!.ToLower();

            if (!Model.Data.TryAdd(key, value))
            {
                Model.Data[key] = value;
            }

            await FuncSave(Model);
        }
    }
}
