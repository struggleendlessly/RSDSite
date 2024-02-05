using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using shared;

namespace web.Client.Pages
{
    public partial class CounterPartialClass
    {
        [Inject]
        IJSRuntime JS { get; set; }

        private int currentCount = 0;

        private void IncrementCount()
        {
            currentCount++;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


            if (firstRender)
            {
                //objRef = DotNetObjectReference.Create(this); onLoadJsNavScroller
                await JS.InvokeVoidAsync(JSInvokeMethodList.onLoad);
            }
        }
    }
}
