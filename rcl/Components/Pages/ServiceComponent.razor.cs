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
    public partial class ServiceComponent
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Parameter]
        public string Key { get; set; } = string.Empty;

        [Parameter]
        public string? SiteName { get; set; }

        public string SiteNameLower { get; set; } = string.Empty;

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        DotNetObjectReference<ServiceComponent>? dotNetHelper { get; set; }

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

            var serviceItemsKey = string.Format(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, SiteNameLower);
            if (!MemoryCache.TryGetValue(serviceItemsKey, out List<ServiceItem> serviceItems))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(SiteNameLower, StaticStrings.ServicesPageServicesListDataJsonFilePath);
                serviceItems = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);

                MemoryCache.Set(serviceItemsKey, serviceItems);
            }

            ServiceItems = serviceItems;
            Model.Data = ServiceItems
                .SelectMany(x => x.LongDesc)
                .Where(x => x.Key == Key)
                .ToDictionary();
        }

        public async Task Save(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var longDesc in serviceItem.LongDesc.ToList())
                {
                    if (model.Data.TryGetValue(longDesc.Key, out var modelData) && modelData != longDesc.Value)
                    {
                        serviceItem.LongDesc[longDesc.Key] = modelData;
                    }
                }
            }

            var jsonModel = JsonConvert.SerializeObject(ServiceItems);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.ServicesPageServicesListDataJsonFilePath, stream);

            var serviceItemsKey = string.Format(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, SiteNameLower);
            MemoryCache.Remove(serviceItemsKey);
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

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
