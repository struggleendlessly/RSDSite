using Microsoft.JSInterop;
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
        public PageModel? PopoversModel { get; set; } = null;

        [Parameter]
        public string? PopoverKey {  get; set; } = null;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private string Value { get; set; } = string.Empty;

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        private bool IsSaving = false;

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, "main", "en", StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

            Value = Model.Data[Key];
        }

        private async Task SaveChangesAsync()
        {
            IsSaving = true;

            Model.Data[Key] = Value;
            await FuncSave(Model);

            await JSRuntime.InvokeVoidAsync(JSInvokeMethodList.closeModal, $"Modal{EditorId}");

            IsSaving = false;
        }
    }
}
