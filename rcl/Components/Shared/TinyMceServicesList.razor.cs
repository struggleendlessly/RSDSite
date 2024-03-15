using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Extensions;

using System.Text;
using Newtonsoft.Json;
using System.Text.Json;

namespace rcl.Components.Shared
{
    public partial class TinyMceServicesList : IDisposable
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

        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public PageModel Model { get; set; } = new PageModel();

        public PageModel ModelUrls { get; set; } = new PageModel();

        DotNetObjectReference<TinyMceServicesList>? dotNetHelper { get; set; }

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

            Model.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();

            ModelUrls.Data = ServiceItems
                .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.ServicesUrlKeyEnding)))
                .ToDictionary();
        }

        public async Task SaveContent(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var shortDesc in serviceItem.ShortDesc.ToList())
                {
                    if (model.Data.TryGetValue(shortDesc.Key, out var modelData) && modelData != shortDesc.Value)
                    {
                        serviceItem.ShortDesc[shortDesc.Key] = modelData;
                    }
                }
            }

            var jsonModel = JsonConvert.SerializeObject(ServiceItems);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.ServicesPageServicesListDataJsonFilePath, stream);

            var serviceItemsKey = string.Format(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, SiteNameLower);
            MemoryCache.Remove(serviceItemsKey);
        }

        public async Task SaveUrl(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var longDesc in serviceItem.LongDesc.Where(x => x.Key.Contains(StaticStrings.ServicesUrlKeyEnding)).ToList())
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

        public async Task Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                Model.Data.Remove(key);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);

                    var jsonModel = JsonConvert.SerializeObject(ServiceItems);

                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
                    await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.ServicesPageServicesListDataJsonFilePath, stream);

                    var serviceItemsKey = string.Format(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, SiteNameLower);
                    MemoryCache.Remove(serviceItemsKey);
                }
            }
        }

        public async Task Add(string key)
        {
            var dateTime = DateTime.Now;
            var serviceItemKey = string.Format(StaticHtmlStrings.ServicesListServiceShortDescDefaultKey, dateTime.ToString("mm"), dateTime.ToString("ss"));
            var serviceItemShortDescValue = StaticHtmlStrings.ServicesListServiceShortDescDefaultValue;

            Model.Data.AddAfter(key, serviceItemKey, serviceItemShortDescValue);

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string> { { serviceItemKey, serviceItemShortDescValue } };
            serviceItem.LongDesc = new Dictionary<string, string>
            {
                { serviceItemKey,  StaticHtmlStrings.ServicesListServiceLongDescDefaultValue },
                { serviceItemKey + StaticStrings.ServicesUrlKeyEnding, serviceItemKey }
            };

            var index = ServiceItems.FindIndex(x => x.ShortDesc.ContainsKey(key));
            ServiceItems.Insert(index + 1, serviceItem);

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
