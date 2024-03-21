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
    public partial class TestimonialsList : IDisposable
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

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public PageModel Model { get; set; } = new PageModel();

        DotNetObjectReference<TestimonialsList>? dotNetHelper { get; set; }

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

            var serviceItemsKey = string.Format(StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, SiteNameLower);
            if (!MemoryCache.TryGetValue(serviceItemsKey, out List<ServiceItem> serviceItems))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(SiteNameLower, StaticStrings.HomePageTestimonialsListDataJsonFilePath);
                serviceItems = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);

                MemoryCache.Set(serviceItemsKey, serviceItems);
            }

            ServiceItems = serviceItems;

            Model.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
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
            await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.HomePageTestimonialsListDataJsonFilePath, stream);

            var serviceItemsKey = string.Format(StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, SiteNameLower);
            MemoryCache.Remove(serviceItemsKey);
        }

        public async Task Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                var serviceAvatarKey = key + StaticStrings.ServicesAvatarKeyEnding;
                var serviceTitleKey = key + StaticStrings.ServicesTitleKeyEnding;
                var serviceSubtitleKey = key + StaticStrings.ServicesSubtitleKeyEnding;
                var serviceTextKey = key + StaticStrings.ServicesTextKeyEnding;
                var serviceImageKey = key + StaticStrings.ServicesImageKeyEnding;

                Model.Data.Remove(key);
                Model.Data.Remove(serviceAvatarKey);
                Model.Data.Remove(serviceTitleKey);
                Model.Data.Remove(serviceSubtitleKey);
                Model.Data.Remove(serviceTextKey);
                Model.Data.Remove(serviceImageKey);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);

                    var jsonModel = JsonConvert.SerializeObject(ServiceItems);

                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
                    await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.HomePageTestimonialsListDataJsonFilePath, stream);

                    var serviceItemsKey = string.Format(StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, SiteNameLower);
                    MemoryCache.Remove(serviceItemsKey);
                }
            }
        }

        public async Task Add(string key)
        {
            var dateTime = DateTime.Now;
            var serviceItemKey = string.Format(StaticHtmlStrings.ServicesListServiceShortDescDefaultKey, dateTime.ToString("mm"), dateTime.ToString("ss"));
            var serviceAvatarKey = serviceItemKey + StaticStrings.ServicesAvatarKeyEnding;
            var serviceTitleKey = serviceItemKey + StaticStrings.ServicesTitleKeyEnding;
            var serviceSubtitleKey = serviceItemKey + StaticStrings.ServicesSubtitleKeyEnding;
            var serviceTextKey = serviceItemKey + StaticStrings.ServicesTextKeyEnding;
            var serviceImageKey = serviceItemKey + StaticStrings.ServicesImageKeyEnding;

            Model.Data.AddAfter(key + StaticStrings.ServicesImageKeyEnding, serviceItemKey, serviceItemKey);
            Model.Data.AddAfter(serviceItemKey, serviceAvatarKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultAvatarValue);
            Model.Data.AddAfter(serviceAvatarKey, serviceTitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTitleValue);
            Model.Data.AddAfter(serviceTitleKey, serviceSubtitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultSubtitleValue);
            Model.Data.AddAfter(serviceSubtitleKey, serviceTextKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTextValue);
            Model.Data.AddAfter(serviceTextKey, serviceImageKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultImageValue);

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string>
            {
                { serviceItemKey, serviceItemKey },
                { serviceAvatarKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultAvatarValue },
                { serviceTitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTitleValue },
                { serviceSubtitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultSubtitleValue },
                { serviceTextKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTextValue },
                { serviceImageKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultImageValue },
            };

            var index = ServiceItems.FindIndex(x => x.ShortDesc.ContainsKey(key));
            ServiceItems.Insert(index + 1, serviceItem);

            var jsonModel = JsonConvert.SerializeObject(ServiceItems);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.HomePageTestimonialsListDataJsonFilePath, stream);

            var serviceItemsKey = string.Format(StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, SiteNameLower);
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
