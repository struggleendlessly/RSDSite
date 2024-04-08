using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;

using System.Text;
using Newtonsoft.Json;
using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class ServicesComponent : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        DotNetObjectReference<ServicesComponent>? dotNetHelper { get; set; }

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
            var key = string.Format(StaticStrings.ServicesPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var blobName = string.Format(StaticStrings.ServicesPageDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var serviceItemsKey = string.Format(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(serviceItemsKey, out List<ServiceItem> serviceItems))
            {
                var blobName = string.Format(StaticStrings.ServicesPageServicesListDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                serviceItems = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);

                MemoryCache.Set(serviceItemsKey, serviceItems);
            }

            ServiceItems = serviceItems;
        }

        [JSInvokable]
        public async Task<string> returnTinyMceImage(JsonElement image)
        {
            var content = image.GetRawText();
            var base64 = content.Replace("\"", "");
            byte[] bytes = Convert.FromBase64String(base64);
            var blobName = $"{StateManager.Lang}/images/{Guid.NewGuid()}.png";

            using (MemoryStream stream = new MemoryStream(bytes))
            return await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);
        }

        public async Task Save(PageModel model)
        {
            var jsonModel = JsonConvert.SerializeObject(model);
            var blobName = string.Format(StaticStrings.ServicesPageDataJsonFilePath, StateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

            var key = string.Format(StaticStrings.ServicesPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            MemoryCache.Remove(key);
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
