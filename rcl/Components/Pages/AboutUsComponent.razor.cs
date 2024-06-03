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
    public partial class AboutUsComponent : IDisposable
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

        [Inject]
        ISubscriptionService SubscriptionService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

        DotNetObjectReference<AboutUsComponent>? dotNetHelper { get; set; }

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

            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AboutUsPageDataJsonMemoryCacheKey, StaticStrings.AboutUsPageDataJsonFilePath);
            PopoversModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversDataJsonMemoryCacheKey, StaticStrings.PopoversDataJsonFilePath, StaticStrings.PopoversContainerName);
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

        public async Task Save(PageModel model)
        {
            await PageDataService.SaveDataAsync(model, StaticStrings.AboutUsPageDataJsonMemoryCacheKey, StaticStrings.AboutUsPageDataJsonFilePath);
            StateHasChanged();
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
