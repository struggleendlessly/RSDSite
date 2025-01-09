using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

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
        public PageModel? PopoversModel { get; set; } = null;

        [Parameter]
        public string? PopoverKey { get; set; } = null;

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
        IApiPageDataService ApiPageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        private bool IsSaving = false;

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

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
            IsSaving = true;

            var content = await JSRuntime.InvokeAsync<string>(JSInvokeMethodList.editorGetContent, EditorId, EditorContentFormat);
            
            Model.Data[Key] = content;
            await FuncSave(Model);

            await ToggleEditModeAsync();

            await JSRuntime.InvokeVoidAsync(JSInvokeMethodList.closeModal, $"Modal{EditorId}");

            IsSaving = false;
        }

        private async Task CancelEditAsync()
        {
            await ToggleEditModeAsync();
        }
    }
}
