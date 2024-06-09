using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;

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
        public string UrlKey { get; set; } = string.Empty;

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        [Inject]
        ISubscriptionService SubscriptionService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public string Key { get; set; } = string.Empty;

        public PageModel Model { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        DotNetObjectReference<ServiceComponent>? dotNetHelper { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetHelper = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync(JSInvokeMethodList.dotNetHelpersSetDotNetHelper, dotNetHelper);
                await JS.InvokeVoidAsync(JSInvokeMethodList.enablePopovers);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await CheckSubscriptionStatus();

            ServiceItems = await PageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StaticStrings.ServicesPageServicesListDataJsonFilePath);
            PopoversModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversMemoryCacheKey, StaticStrings.PopoversDataJsonFilePath, StaticStrings.PopoversContainerName);

            var keyValuePairUrl = ServiceItems.SelectMany(x => x.LongDesc).FirstOrDefault(x => x.Value == UrlKey);
            if (string.IsNullOrWhiteSpace(keyValuePairUrl.Key) || string.IsNullOrWhiteSpace(keyValuePairUrl.Value))
            {
                // TODO: Add a redirect to the 404 page
                return;
            }

            Key = keyValuePairUrl.Key.Replace(StaticStrings.UrlKeyEnding, string.Empty);
            var titleKey = Key + StaticStrings.TitleKeyEnding;
            var subtitleKey = Key + StaticStrings.SubtitleKeyEnding;
            var imageKey = Key + StaticStrings.ImageKeyEnding;

            Model.Data = ServiceItems
                .SelectMany(x => x.LongDesc)
                .Where(x => x.Key == Key || x.Key == titleKey || x.Key == subtitleKey || x.Key == imageKey)
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

            await PageDataService.SaveDataAsync(ServiceItems, StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StaticStrings.ServicesPageServicesListDataJsonFilePath);
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

        public async Task CheckSubscriptionStatus()
        {
            var authenticationState = await AuthenticationStateTask;
            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                var isSubscriptionActive = await SubscriptionService.IsWebsiteSubscriptionActiveAsync();
                if (!isSubscriptionActive)
                {
                    NavigationManager.NavigateTo(StateManager.GetPageUrl(StaticRoutesStrings.SubscriptionErrorUrl));
                }
            }
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
