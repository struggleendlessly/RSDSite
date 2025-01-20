using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

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
        public string PropertyName { get; set; } = nameof(PageModel.Data);

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
        IApiPageDataService ApiPageDataService { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        private bool IsSaving = false;

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

            Value = GetPropertyValue(Model, PropertyName, Key);
        }

        private async Task SaveChangesAsync()
        {
            IsSaving = true;

            SetPropertyValue(Model, PropertyName, Key, Value);
            await FuncSave(Model);

            await JSRuntime.InvokeVoidAsync(JSInvokeMethodList.closeModal, $"Modal{EditorId}");

            IsSaving = false;
        }

        private string GetPropertyValue(PageModel model, string propertyName, string key)
        {
            if (propertyName == nameof(PageModel.Data))
            {
                return model.Data.ContainsKey(key) ? model.Data[key] : string.Empty;
            }
            else if (propertyName == nameof(PageModel.SEO))
            {
                return model.SEO.ContainsKey(key) ? model.SEO[key] : string.Empty;
            }

            return string.Empty;
        }

        private void SetPropertyValue(PageModel model, string propertyName, string key, string value)
        {
            if (propertyName == nameof(PageModel.Data))
            {
                model.Data[key] = value;
            }
            else if (propertyName == nameof(PageModel.SEO))
            {
                model.SEO[key] = value;
            }
        }
    }
}
