using Microsoft.JSInterop;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Models;
using shared.Models.API;
using shared.Interfaces;
using shared.Data.Entities;
using shared.Interfaces.Api;
using shared.ConfigurationOptions;

using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class ContactUsComponent : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        IApiAzureBlobStorageService ApiAzureBlobStorageService { get; set; }

        [Inject]
        IOptions<LeafletOptions> LeafletOptions { get; set; }

        [Inject]
        IApiWebsiteService ApiWebsiteService { get; set; }

        [Inject]
        IApiContactUsMessageService ApiContactUsMessageService { get; set; }

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

        [SupplyParameterFromForm]
        private ContactUsMessageModel Input { get; set; } = new();

        DotNetObjectReference<ContactUsComponent>? dotNetHelper { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetHelper = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync(JSInvokeMethodList.dotNetHelpersSetDotNetHelper, dotNetHelper);
                //await JS.InvokeVoidAsync(JSInvokeMethodList.leafletActivate, LeafletOptions.Value.AccessToken, Model.Data[StaticStrings.ContactUsPageDatMapCoordinatesKey], Model.Data[StaticStrings.ContactUsPageDataMapMarkerTextKey]);
                await JS.InvokeVoidAsync(JSInvokeMethodList.enablePopovers);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await CheckSubscriptionStatus();

            Model = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.ContactUsPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.ContactUsPageDataJsonFilePath);
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
            await ApiPageDataService.SaveDataAsync(model, StaticStrings.ContactUsPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.ContactUsPageDataJsonFilePath);
        }

        public async Task SubmitForm()
        {
            var currentWebsite = await ApiWebsiteService.GetWebsiteByName(StateManager.SiteName);
            var message = new ContactUsMessage
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                Details = Input.Details,
                Created = DateTime.UtcNow,
                WebsiteId = currentWebsite.Id
            };

            await ApiContactUsMessageService.CreateAsync(message);

            await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.ContactUsFormAlertId, StaticHtmlStrings.CSSAlertSuccess, LocalizationModel.Data[StaticStrings.Localization_ContactUs_Form_Success_Message_Key]);

            Input = new ContactUsMessageModel();
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
