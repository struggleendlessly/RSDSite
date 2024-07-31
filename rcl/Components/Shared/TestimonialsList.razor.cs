using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Extensions;
using shared.Interfaces;

using System.Text.Json;

namespace rcl.Components.Shared
{
    public partial class TestimonialsList : IDisposable
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

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public PageModel Model { get; set; } = new PageModel();

        DotNetObjectReference<TestimonialsList>? dotNetHelper { get; set; }

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
            ServiceItems = await PageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, StaticStrings.HomePageTestimonialsListDataJsonFilePath);

            Model.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();
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

            await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, StaticStrings.HomePageTestimonialsListDataJsonFilePath);
        }

        public async Task Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                var serviceAvatarKey = key + StaticStrings.AvatarKeyEnding;
                var serviceTitleKey = key + StaticStrings.TitleKeyEnding;
                var serviceSubtitleKey = key + StaticStrings.SubtitleKeyEnding;
                var serviceTextKey = key + StaticStrings.TextKeyEnding;
                var serviceIsVisibleKey = key + StaticStrings.IsVisibleKeyEnding;

                Model.Data.Remove(key);
                Model.Data.Remove(serviceAvatarKey);
                Model.Data.Remove(serviceTitleKey);
                Model.Data.Remove(serviceSubtitleKey);
                Model.Data.Remove(serviceTextKey);
                Model.Data.Remove(serviceIsVisibleKey);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);

                    await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, StaticStrings.HomePageTestimonialsListDataJsonFilePath);
                }
            }
        }

        public async Task Add(string? key = null)
        {
            isAdding = true;

            var dateTime = DateTime.Now;
            var serviceItemKey = string.Format(StaticHtmlStrings.ServicesListServiceShortDescDefaultKey, dateTime.ToString("mm"), dateTime.ToString("ss"));
            var serviceAvatarKey = serviceItemKey + StaticStrings.AvatarKeyEnding;
            var serviceTitleKey = serviceItemKey + StaticStrings.TitleKeyEnding;
            var serviceSubtitleKey = serviceItemKey + StaticStrings.SubtitleKeyEnding;
            var serviceTextKey = serviceItemKey + StaticStrings.TextKeyEnding;
            var serviceIsVisibleKey = serviceItemKey + StaticStrings.IsVisibleKeyEnding;

            Model.Data.AddAfter(key + StaticStrings.ImageKeyEnding, serviceItemKey, serviceItemKey);
            Model.Data.AddAfter(serviceItemKey, serviceAvatarKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultAvatarValue);
            Model.Data.AddAfter(serviceAvatarKey, serviceTitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTitleValue);
            Model.Data.AddAfter(serviceTitleKey, serviceSubtitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultSubtitleValue);
            Model.Data.AddAfter(serviceSubtitleKey, serviceTextKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTextValue);
            Model.Data.AddAfter(serviceTextKey, serviceIsVisibleKey, bool.TrueString.ToLower());

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string>
            {
                { serviceItemKey, serviceItemKey },
                { serviceAvatarKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultAvatarValue },
                { serviceTitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTitleValue },
                { serviceSubtitleKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultSubtitleValue },
                { serviceTextKey, StaticHtmlStrings.HomeTestimonialsListServiceShortDescDefaultTextValue },
                { serviceIsVisibleKey, bool.TrueString.ToLower() }
            };

            if (!string.IsNullOrWhiteSpace(key))
            {
                var index = ServiceItems.FindIndex(x => x.ShortDesc.ContainsKey(key));
                ServiceItems.Insert(index + 1, serviceItem);
            }
            else
            {
                ServiceItems.Add(serviceItem);
            }

            await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.HomePageTestimonialsListDataJsonMemoryCacheKey, StaticStrings.HomePageTestimonialsListDataJsonFilePath);

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
