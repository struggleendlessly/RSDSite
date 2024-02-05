using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using shared;

namespace web.Components.Pages
{
    public partial class Home : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        public void Dispose()
        {
            //throw new NotImplementedException();
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
