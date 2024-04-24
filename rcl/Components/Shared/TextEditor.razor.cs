using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Shared
{
    public partial class TextEditor
    {
        [Parameter]
        public string EditorId { get; set; } = string.Empty;

        [Parameter]
        public string EditorType { get; set; } = string.Empty;

        [Parameter]
        public Func<PageModel, Task> FuncSave { get; set; }

        [Parameter]
        public PageModel Model { get; set; } = new PageModel();

        [Parameter]
        public string Key { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private string Value { get; set; } = string.Empty;

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel SettingsModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            SettingsModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
            Value = Model.Data[Key];
        }

        private async Task SaveChangesAsync()
        {
            Model.Data[Key] = Value;
            await FuncSave(Model);
        }
    }
}
