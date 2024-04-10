using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Extensions;
using shared.Interfaces;

using System.Text;
using Newtonsoft.Json;
using System.Text.Json;

namespace rcl.Components.Shared
{
    public partial class TeamList : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public PageModel Model { get; set; } = new PageModel();

        DotNetObjectReference<TeamList>? dotNetHelper { get; set; }

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
            ServiceItems = await PageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StaticStrings.AboutUsPageTeamListDataJsonFilePath);

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
            var blobName = string.Format(StaticStrings.AboutUsPageTeamListDataJsonFilePath, StateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

            var serviceItemsKey = string.Format(StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            MemoryCache.Remove(serviceItemsKey);
        }

        public async Task Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                var serviceImageKey = key + StaticStrings.ImageKeyEnding;
                var serviceTitleKey = key + StaticStrings.TitleKeyEnding;
                var serviceSubtitleKey = key + StaticStrings.SubtitleKeyEnding;
                var serviceTextKey = key + StaticStrings.TextKeyEnding;

                Model.Data.Remove(key);
                Model.Data.Remove(serviceImageKey);
                Model.Data.Remove(serviceTitleKey);
                Model.Data.Remove(serviceSubtitleKey);
                Model.Data.Remove(serviceTextKey);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);

                    var jsonModel = JsonConvert.SerializeObject(ServiceItems);
                    var blobName = string.Format(StaticStrings.AboutUsPageTeamListDataJsonFilePath, StateManager.Lang);

                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
                    await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

                    var serviceItemsKey = string.Format(StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
                    MemoryCache.Remove(serviceItemsKey);
                }
            }
        }

        public async Task Add(string key)
        {
            var dateTime = DateTime.Now;
            var serviceItemKey = string.Format(StaticHtmlStrings.ServicesListServiceShortDescDefaultKey, dateTime.ToString("mm"), dateTime.ToString("ss"));
            var serviceImageKey = serviceItemKey + StaticStrings.ImageKeyEnding;
            var serviceTitleKey = serviceItemKey + StaticStrings.TitleKeyEnding;
            var serviceSubtitleKey = serviceItemKey + StaticStrings.SubtitleKeyEnding;
            var serviceTextKey = serviceItemKey + StaticStrings.TextKeyEnding;

            Model.Data.AddAfter(key + StaticStrings.TextKeyEnding, serviceItemKey, serviceItemKey);
            Model.Data.AddAfter(serviceItemKey, serviceImageKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultImageValue);
            Model.Data.AddAfter(serviceImageKey, serviceTitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTitleValue);
            Model.Data.AddAfter(serviceTitleKey, serviceSubtitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultSubtitleValue);
            Model.Data.AddAfter(serviceSubtitleKey, serviceTextKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTextValue);

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string>
            {
                { serviceItemKey, serviceItemKey },
                { serviceImageKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultImageValue },
                { serviceTitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTitleValue },
                { serviceSubtitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultSubtitleValue },
                { serviceTextKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTextValue }
            };

            var index = ServiceItems.FindIndex(x => x.ShortDesc.ContainsKey(key));
            ServiceItems.Insert(index + 1, serviceItem);

            var jsonModel = JsonConvert.SerializeObject(ServiceItems);
            var blobName = string.Format(StaticStrings.AboutUsPageTeamListDataJsonFilePath, StateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

            var serviceItemsKey = string.Format(StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            MemoryCache.Remove(serviceItemsKey);
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

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
