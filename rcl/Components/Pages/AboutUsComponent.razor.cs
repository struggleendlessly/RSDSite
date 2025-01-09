using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Models;
using shared.Models.API;
using shared.Interfaces;
using shared.Interfaces.Api;

using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class AboutUsComponent : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        IApiAzureBlobStorageService ApiAzureBlobStorageService { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        [Inject]
        IApiSubscriptionService ApiSubscriptionService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

        public PageModel LocalizationModel { get; set; } = new PageModel();

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

            Model = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.AboutUsPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AboutUsPageDataJsonFilePath);
            PopoversModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.PopoversDataJsonFilePath, StaticStrings.PopoversContainerName);
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
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

        public async Task Save(PageModel model)
        {
            await ApiPageDataService.SaveDataAsync(model, StaticStrings.AboutUsPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AboutUsPageDataJsonFilePath);
            StateHasChanged();
        }

        public async Task CheckSubscriptionStatus()
        {
            var authenticationState = await AuthenticationStateTask;
            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                var isSubscriptionActive = await ApiSubscriptionService.IsWebsiteSubscriptionActiveAsync(StateManager.SiteName);
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
