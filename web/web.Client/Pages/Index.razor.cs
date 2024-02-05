using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using shared;

namespace web.Client.Pages
{
    public partial class Index
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
