using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using shared;
using shared.Models;

namespace web.Components.Shared
{
    public partial class TinyMceEditor
    {
        [Parameter]
        public string TinyMceId { get; set; } = string.Empty;

        [Parameter]
        public string TinyMceContentFormat { get; set; } = string.Empty;

        [Parameter]
        public Func<PageModel, bool> FuncSave { get; set; }

        [Parameter]
        public PageModel Model { get; set; }

        [Parameter]
        public string Key { get; set; } = string.Empty;   

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private bool EditMode { get; set; } = false;

        private string Value { get; set; } = string.Empty;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Value = Model.Data[Key];
        }

        private async Task ToggleEditModeAsync()
        {
            EditMode = !EditMode;

            if (EditMode)
            {
                await InitializeTinyMCEAsync();
            }
            else
            {
                await DestroyTinyMCEAsync();
            }
        }

        private async Task InitializeTinyMCEAsync()
        {
            await JSRuntime.InvokeVoidAsync(JSInvokeMethodList.tinymceActivate, TinyMceId);
        }

        private async Task DestroyTinyMCEAsync()
        {
            await JSRuntime.InvokeVoidAsync(JSInvokeMethodList.tinymceDestroy, TinyMceId);
        }

        private async Task SaveChangesAsync()
        {
            var content = await JSRuntime.InvokeAsync<string>(JSInvokeMethodList.tinymceGetContent, TinyMceId, TinyMceContentFormat);

            Model.Data[Key] = content;
            FuncSave(Model);

            await ToggleEditModeAsync();
        }

        private async Task CancelEditAsync()
        {
            await ToggleEditModeAsync();
        }
    }
}
