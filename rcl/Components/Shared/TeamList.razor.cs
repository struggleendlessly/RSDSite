using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Models.API;
using shared.Extensions;
using shared.Interfaces;
using shared.Interfaces.Api;

using System.Text.Json;

namespace rcl.Components.Shared
{
    public partial class TeamList : IDisposable
    {
        [Parameter]
        public PageModel? PopoversModel { get; set; } = null;

        [Parameter]
        public string? PopoverKey { get; set; } = null;

        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        IApiAzureBlobStorageService ApiAzureBlobStorageService { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public PageModel Model { get; set; } = new PageModel();

        DotNetObjectReference<TeamList>? dotNetHelper { get; set; }

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
            ServiceItems = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AboutUsPageTeamListDataJsonFilePath);

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

            await ApiPageDataService.SaveDataAsync(ServiceItems, StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AboutUsPageTeamListDataJsonFilePath);
        }

        public async Task Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                var serviceImageKey = key + StaticStrings.ImageKeyEnding;
                var serviceTitleKey = key + StaticStrings.TitleKeyEnding;
                var serviceSubtitleKey = key + StaticStrings.SubtitleKeyEnding;
                var serviceTextKey = key + StaticStrings.TextKeyEnding;
                var serviceIsVisibleKey = key + StaticStrings.IsVisibleKeyEnding;

                Model.Data.Remove(key);
                Model.Data.Remove(serviceImageKey);
                Model.Data.Remove(serviceTitleKey);
                Model.Data.Remove(serviceSubtitleKey);
                Model.Data.Remove(serviceTextKey);
                Model.Data.Remove(serviceIsVisibleKey);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);

                    await ApiPageDataService.SaveDataAsync(ServiceItems, StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AboutUsPageTeamListDataJsonFilePath);
                }
            }
        }

        public async Task Add(string? key = null)
        {
            isAdding = true;

            var dateTime = DateTime.Now;
            var serviceItemKey = string.Format(StaticHtmlStrings.ServicesListServiceShortDescDefaultKey, dateTime.ToString("mm"), dateTime.ToString("ss"));
            var serviceImageKey = serviceItemKey + StaticStrings.ImageKeyEnding;
            var serviceTitleKey = serviceItemKey + StaticStrings.TitleKeyEnding;
            var serviceSubtitleKey = serviceItemKey + StaticStrings.SubtitleKeyEnding;
            var serviceTextKey = serviceItemKey + StaticStrings.TextKeyEnding;
            var serviceIsVisibleKey = serviceItemKey + StaticStrings.IsVisibleKeyEnding;

            Model.Data.AddAfter(key + StaticStrings.TextKeyEnding, serviceItemKey, serviceItemKey);
            Model.Data.AddAfter(serviceItemKey, serviceImageKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultImageValue);
            Model.Data.AddAfter(serviceImageKey, serviceTitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTitleValue);
            Model.Data.AddAfter(serviceTitleKey, serviceSubtitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultSubtitleValue);
            Model.Data.AddAfter(serviceSubtitleKey, serviceTextKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTextValue);
            Model.Data.AddAfter(serviceTextKey, serviceIsVisibleKey, bool.TrueString.ToLower());

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string>
            {
                { serviceItemKey, serviceItemKey },
                { serviceImageKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultImageValue },
                { serviceTitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTitleValue },
                { serviceSubtitleKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultSubtitleValue },
                { serviceTextKey, StaticHtmlStrings.AboutUsTeamListServiceShortDescDefaultTextValue },
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

            await ApiPageDataService.SaveDataAsync(ServiceItems, StaticStrings.AboutUsPageTeamListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AboutUsPageTeamListDataJsonFilePath);

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
            var uploadFileModel = new UploadFileModel()
            {
                SiteName = StateManager.SiteName,
                BlobName = blobName,
                FileData = bytes
            };

            var result = await ApiAzureBlobStorageService.UploadFileAsync(uploadFileModel);
            return result;
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
