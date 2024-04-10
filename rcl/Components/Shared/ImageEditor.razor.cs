using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Shared
{
    public partial class ImageEditor
    {
        [Parameter]
        public string EditorId { get; set; } = string.Empty;

        [Parameter]
        public Func<PageModel, Task> FuncSave { get; set; }

        [Parameter]
        public PageModel Model { get; set; } = new PageModel();

        [Parameter]
        public string Key { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        private async Task SaveChangesAsync()
        {
            var content = await JSRuntime.InvokeAsync<string>(JSInvokeMethodList.imageEditorGetContent, EditorId);

            if (!string.IsNullOrWhiteSpace(content))
            {
                Model.Data[Key] = content;
                await FuncSave(Model);
            }
        }
    }
}
