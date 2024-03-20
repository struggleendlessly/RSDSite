using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;

using System.Text;
using Newtonsoft.Json;
using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class HomeComponent : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Parameter]
        public string? SiteName { get; set; }

        public string SiteNameLower { get; set; } = string.Empty;

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        DotNetObjectReference<HomeComponent>? dotNetHelper { get; set; }

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
            SiteNameLower = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.DefaultSiteName : SiteName.ToLower();
            var key = string.Format(StaticStrings.HomePageDataJsonMemoryCacheKey, SiteNameLower);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(SiteNameLower, StaticStrings.HomePageDataJsonFilePath);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var serviceItemsKey = string.Format(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, SiteNameLower);
            if (!MemoryCache.TryGetValue(serviceItemsKey, out List<ServiceItem> serviceItems))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(SiteNameLower, StaticStrings.ServicesPageServicesListDataJsonFilePath);
                serviceItems = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);
            }

            ServiceItems = serviceItems.Take(4).ToList();
        }

        [JSInvokable]
        public async Task<string> returnTinyMceImage(JsonElement image)
        {
            var content = image.GetRawText();
            var base64 = content.Replace("\"", "");
            byte[] bytes = Convert.FromBase64String(base64);
            var blobName = $"images/{Guid.NewGuid()}.png";

            using (MemoryStream stream = new MemoryStream(bytes))
            return await BlobStorageManager.UploadFile(SiteNameLower, blobName, stream);
        }

        public async Task Save(PageModel model)
        {
            var jsonModel = JsonConvert.SerializeObject(model);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.HomePageDataJsonFilePath, stream);

            var key = string.Format(StaticStrings.HomePageDataJsonMemoryCacheKey, SiteNameLower);
            MemoryCache.Remove(key);
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
