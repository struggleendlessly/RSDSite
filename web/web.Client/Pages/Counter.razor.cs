using System;
using System.Threading.Tasks;

namespace web.Client.Pages
{
    public partial class Counter
	{
		private int currentCount = 0;

		private void IncrementCount()
		{
			currentCount++;
		}

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
