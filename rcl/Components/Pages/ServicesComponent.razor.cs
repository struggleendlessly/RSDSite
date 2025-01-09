using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Enums;
using shared.Models;
using shared.Models.API;
using shared.Interfaces;
using shared.Interfaces.Api;

using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class ServicesComponent : IDisposable
    {
        [Parameter]
        public ServicesPageType PageType { get; set; }

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

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        DotNetObjectReference<ServicesComponent>? dotNetHelper { get; set; }

        public string ServicesPageKeyEnding = string.Empty;
        public string ServicesPageDataJsonFilePath {  get; set; } = string.Empty;
        public string ServicesPageServicesListDataJsonFilePath { get; set; } = string.Empty;

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
            SetJSONPaths();

            Model = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.ServicesPageDataJsonMemoryCacheKey + ServicesPageKeyEnding, StateManager.SiteName, StateManager.Lang, ServicesPageDataJsonFilePath);
            PopoversModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.PopoversDataJsonFilePath, StaticStrings.PopoversContainerName);
            ServiceItems = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey + ServicesPageKeyEnding, StateManager.SiteName, StateManager.Lang, ServicesPageServicesListDataJsonFilePath);
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
            await ApiPageDataService.SaveDataAsync(model, StaticStrings.ServicesPageDataJsonMemoryCacheKey + ServicesPageKeyEnding, StateManager.SiteName, StateManager.Lang, ServicesPageDataJsonFilePath);
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

        public void SetJSONPaths()
        {
            if (PageType == ServicesPageType.Services)
            {
                ServicesPageKeyEnding = string.Empty;
                ServicesPageDataJsonFilePath = StaticStrings.ServicesPageDataJsonFilePath;
                ServicesPageServicesListDataJsonFilePath = StaticStrings.ServicesPageServicesListDataJsonFilePath;
            }
            else if (PageType == ServicesPageType.Portfolio)
            {
                ServicesPageKeyEnding = StaticStrings.PortfolioPageKeyEnding;
                ServicesPageDataJsonFilePath = StaticStrings.PortfolioPageDataJsonFilePath;
                ServicesPageServicesListDataJsonFilePath = StaticStrings.PortfolioPageServicesListDataJsonFilePath;
            }
            else if (PageType == ServicesPageType.Documents)
            {
                ServicesPageKeyEnding = StaticStrings.DocumentsPageKeyEnding;
                ServicesPageDataJsonFilePath = StaticStrings.DocumentsPageDataJsonFilePath;
                ServicesPageServicesListDataJsonFilePath = StaticStrings.DocumentsPageServicesListDataJsonFilePath;
            }
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
