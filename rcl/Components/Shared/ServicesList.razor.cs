using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Enums;
using shared.Models;
using shared.Managers;
using shared.Extensions;
using shared.Interfaces;

using System.Text.Json;

namespace rcl.Components.Shared
{
    public partial class ServicesList : IDisposable
    {
        [Parameter]
        public PageModel? PopoversModel { get; set; } = null;

        [Parameter]
        public string? PopoverKey { get; set; } = null;

        [Parameter]
        public ServicesPageType PageType { get; set; }

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

        public string ServicesListKeyEnding = string.Empty;
        public string ServicesListDataJsonFilePath { get; set; } = string.Empty;

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
            SetJSONPaths();

            Model.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();

            ModelUrls.Data = ServiceItems
                .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.UrlKeyEnding)))
                .ToDictionary();

            //SettingsModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
            //LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
        }

        public async Task SaveContent(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                var serviceItemKey = serviceItem.ShortDesc.FirstOrDefault().Key;

                foreach (var modelDataEntry in model.Data)
                {
                    if (serviceItem.ShortDesc.TryGetValue(modelDataEntry.Key, out var shortDescValue))
                    {
                        if (modelDataEntry.Value != shortDescValue)
                        {
                            serviceItem.ShortDesc[modelDataEntry.Key] = modelDataEntry.Value;
                        }
                    }
                    else if (modelDataEntry.Key.Contains(serviceItemKey))
                    {
                        serviceItem.ShortDesc[modelDataEntry.Key] = modelDataEntry.Value;
                    }
                }
            }

            //await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey + ServicesListKeyEnding, ServicesListDataJsonFilePath);
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

            //await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey + ServicesListKeyEnding, ServicesListDataJsonFilePath);
            StateHasChanged();
        }

        public async Task Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                var serviceTitleKey = key + StaticStrings.TitleKeyEnding;
                var serviceSubtitleKey = key + StaticStrings.SubtitleKeyEnding;
                var serviceIsVisibleKey = key + StaticStrings.IsVisibleKeyEnding;

                Model.Data.Remove(key);
                Model.Data.Remove(serviceTitleKey);
                Model.Data.Remove(serviceSubtitleKey);
                Model.Data.Remove(serviceIsVisibleKey);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);

                    //await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey + ServicesListKeyEnding, ServicesListDataJsonFilePath);
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
            var serviceIsVisibleKey = serviceItemKey + StaticStrings.IsVisibleKeyEnding;

            Model.Data.AddAfter(key + StaticStrings.SubtitleKeyEnding, serviceItemKey, serviceItemKey);
            Model.Data.AddAfter(serviceItemKey, serviceTitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultTitleValue);
            Model.Data.AddAfter(serviceTitleKey, serviceSubtitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultSubtitleValue);
            Model.Data.AddAfter(serviceSubtitleKey, serviceIsVisibleKey, bool.TrueString.ToLower());

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string> 
            { 
                { serviceItemKey, serviceItemKey },
                { serviceTitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultTitleValue },
                { serviceSubtitleKey, StaticHtmlStrings.ItemsListItemShortDescDefaultSubtitleValue },
                { serviceIsVisibleKey, bool.TrueString.ToLower() }
            };
            serviceItem.LongDesc = new Dictionary<string, string>
            {
                { serviceItemKey,  StaticHtmlStrings.ItemsListItemLongDescDefaultValue },
                { serviceItemKey + StaticStrings.UrlKeyEnding, serviceItemKey },
                { serviceItemKey + StaticStrings.TitleKeyEnding, StaticHtmlStrings.ItemsListItemLongDescTitleDefaultValue },
                { serviceItemKey + StaticStrings.SubtitleKeyEnding, StaticHtmlStrings.ItemsListItemLongDescSubtitleDefaultValue },
                { serviceItemKey + StaticStrings.ImageKeyEnding, StaticHtmlStrings.ItemsListItemLongDescImageDefaultValue }
            };
            serviceItem.SEO = new Dictionary<string, string>
            {
                { serviceItemKey + StaticStrings.TitleKeyEnding, StaticHtmlStrings.ItemsListItemSEODefaultTitleValue },
                { serviceItemKey + StaticStrings.MetaDescriptionKeyEnding, StaticHtmlStrings.ItemsListItemSEODefaultMetaDescriptionValue }
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

            //await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey + ServicesListKeyEnding, ServicesListDataJsonFilePath);

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

        public void SetJSONPaths()
        {
            if (PageType == ServicesPageType.Services)
            {
                ServicesListKeyEnding = string.Empty;
                ServicesListDataJsonFilePath = StaticStrings.ServicesPageServicesListDataJsonFilePath;
            }
            else if (PageType == ServicesPageType.Portfolio)
            {
                ServicesListKeyEnding = StaticStrings.PortfolioPageKeyEnding;
                ServicesListDataJsonFilePath = StaticStrings.PortfolioPageServicesListDataJsonFilePath;
            }
            else if (PageType == ServicesPageType.Documents)
            {
                ServicesListKeyEnding = StaticStrings.DocumentsPageKeyEnding;
                ServicesListDataJsonFilePath = StaticStrings.DocumentsPageServicesListDataJsonFilePath;
            }
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
