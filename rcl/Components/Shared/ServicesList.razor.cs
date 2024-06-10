using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Extensions;

using System.Text.Json;
using shared.Interfaces;

namespace rcl.Components.Shared
{
    public partial class ServicesList : IDisposable
    {
        [Parameter]
        public PageModel? PopoversModel { get; set; } = null;

        [Parameter]
        public string? PopoverKey { get; set; } = null;

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

        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public PageModel Model { get; set; } = new PageModel();

        public PageModel ModelUrls { get; set; } = new PageModel();

        public PageModel SettingsModel { get; set; } = new PageModel();

        public PageModel LocalizationModel { get; set; } = new PageModel();

        DotNetObjectReference<ServicesList>? dotNetHelper { get; set; }

        private bool isAdding = false;

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
            Model.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();

            ModelUrls.Data = ServiceItems
                .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.UrlKeyEnding)))
                .ToDictionary();

            SettingsModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
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

            await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StaticStrings.ServicesPageServicesListDataJsonFilePath);
        }

        public async Task SaveUrl(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var longDesc in serviceItem.LongDesc.Where(x => x.Key.Contains(StaticStrings.UrlKeyEnding)).ToList())
                {
                    if (model.Data.TryGetValue(longDesc.Key, out var modelData) && modelData != longDesc.Value)
                    {
                        serviceItem.LongDesc[longDesc.Key] = modelData;
                    }
                }
            }

            await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StaticStrings.ServicesPageServicesListDataJsonFilePath);
            StateHasChanged();
        }

        public async Task Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                var serviceTitleKey = key + StaticStrings.TitleKeyEnding;
                var serviceSubtitleKey = key + StaticStrings.SubtitleKeyEnding;

                Model.Data.Remove(key);
                Model.Data.Remove(serviceTitleKey);
                Model.Data.Remove(serviceSubtitleKey);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);

                    await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StaticStrings.ServicesPageServicesListDataJsonFilePath);
                }
            }
        }

        public async Task Add(string? key = null)
        {
            isAdding = true;

            var dateTime = DateTime.Now;
            var serviceItemKey = string.Format(StaticHtmlStrings.ItemsListItemShortDescDefaultKey, dateTime.ToString("mm"), dateTime.ToString("ss"));
            var serviceTitleKey = serviceItemKey + StaticStrings.TitleKeyEnding;
            var serviceSubtitleKey = serviceItemKey + StaticStrings.SubtitleKeyEnding;

            Model.Data.AddAfter(key + StaticStrings.SubtitleKeyEnding, serviceItemKey, serviceItemKey);
            Model.Data.AddAfter(serviceItemKey, serviceTitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultTitleValue);
            Model.Data.AddAfter(serviceTitleKey, serviceSubtitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultSubtitleValue);

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string> 
            { 
                { serviceItemKey, serviceItemKey },
                { serviceTitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultTitleValue },
                { serviceSubtitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultSubtitleValue }
            };
            serviceItem.LongDesc = new Dictionary<string, string>
            {
                { serviceItemKey,  StaticHtmlStrings.ItemsListItemLongDescDefaultValue },
                { serviceItemKey + StaticStrings.UrlKeyEnding, serviceItemKey },
                { serviceItemKey + StaticStrings.TitleKeyEnding, StaticHtmlStrings.ItemsListItemLongDescTitleDefaultValue },
                { serviceItemKey + StaticStrings.SubtitleKeyEnding, StaticHtmlStrings.ItemsListItemLongDescSubtitleDefaultValue },
                { serviceItemKey + StaticStrings.ImageKeyEnding, StaticHtmlStrings.ItemsListItemLongDescImageDefaultValue }
            };

            ModelUrls.Data.Add(serviceItemKey + StaticStrings.UrlKeyEnding, serviceItemKey);

            if (!string.IsNullOrWhiteSpace(key))
            {
                var index = ServiceItems.FindIndex(x => x.ShortDesc.ContainsKey(key));
                ServiceItems.Insert(index + 1, serviceItem);
            }
            else
            {
                ServiceItems.Add(serviceItem);
            }

            await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StaticStrings.ServicesPageServicesListDataJsonFilePath);

            await Task.Delay(1000);

            isAdding = false;
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
