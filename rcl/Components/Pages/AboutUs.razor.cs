using Microsoft.JSInterop;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Managers;

using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class AboutUs : IDisposable
    {
        [Inject]
        IWebHostEnvironment hostingEnvironment { get; set; }

        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager blobStorageManager { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        DotNetObjectReference<AboutUs>? dotNetHelper { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetHelper = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync(JSInvokeMethodList.dotNetHelpersSetDotNetHelper, dotNetHelper);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Model = JsonFileManager.ReadFromJsonFile<PageModel>(hostingEnvironment.WebRootPath, StaticStrings.AboutUsPageDataJsonFilePath);
        }

        [JSInvokable]
        public async Task<string> returnTinyMceImage(JsonElement image)
        {
            var content = image.GetRawText();
            var base64 = content.Replace("\"", "");
            var blobName = $"{Guid.NewGuid()}.png";

            return await blobStorageManager.UploadImageAsync(base64, blobName);
        }

        public bool Save(PageModel model)
        {
            JsonFileManager.WriteToJsonFile(model, hostingEnvironment.WebRootPath, StaticStrings.AboutUsPageDataJsonFilePath);

            return true;
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
