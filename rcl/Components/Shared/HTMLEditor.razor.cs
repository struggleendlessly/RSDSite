using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Shared
{
    public partial class HTMLEditor
    {
        [Parameter]
        public string EditorId { get; set; } = string.Empty;

        [Parameter]
        public string EditorContentFormat { get; set; } = string.Empty;

        [Parameter]
        public Func<PageModel, Task> FuncSave { get; set; }

        [Parameter]
        public PageModel Model { get; set; } = new PageModel();

        [Parameter]
        public string Key { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private bool EditMode { get; set; } = false;

        private string Value { get; set; } = string.Empty;

        private bool IsFirstRender { get; set; } = true;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

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

        private async Task ToggleEditModeAsync()
        {
            EditMode = !EditMode;

            if (EditMode && IsFirstRender)
            {
                await InitializeEditorAsync();
                IsFirstRender = false;
            }
        }

        private async Task InitializeEditorAsync()
        {
            await JSRuntime.InvokeVoidAsync(JSInvokeMethodList.editorActivate, EditorId);
        }

        private string GetVisibilityClass()
        {
            if (EditMode)
            {
                return StaticHtmlStrings.CSSDisplayBlock;
            }
            else
            {
                return StaticHtmlStrings.CSSDisplayNone;
            }
        }

        private async Task SaveChangesAsync()
        {
            var content = await JSRuntime.InvokeAsync<string>(JSInvokeMethodList.editorGetContent, EditorId, EditorContentFormat);

            Model.Data[Key] = content;
            await FuncSave(Model);

            await ToggleEditModeAsync();
        }

        private async Task CancelEditAsync()
        {
            await ToggleEditModeAsync();
        }
    }
}
