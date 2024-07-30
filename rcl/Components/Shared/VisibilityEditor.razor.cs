using shared;
using shared.Models;
using shared.Interfaces;

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

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {     
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
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
