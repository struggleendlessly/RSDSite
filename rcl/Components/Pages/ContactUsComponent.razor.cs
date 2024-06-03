using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;
using shared.Data.Entities;

using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class ContactUsComponent : IDisposable
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Inject]
        private IConfiguration Configuration { get; set; }

        [Inject]
        IWebsiteService WebsiteService { get; set; }

        [Inject]
        IContactUsMessageService ContactUsMessageService { get; set; }

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

        [SupplyParameterFromForm]
        private ContactUsMessageModel Input { get; set; } = new();

        DotNetObjectReference<ContactUsComponent>? dotNetHelper { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetHelper = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync(JSInvokeMethodList.dotNetHelpersSetDotNetHelper, dotNetHelper);
                await JS.InvokeVoidAsync(JSInvokeMethodList.leafletActivate, Configuration["Leaflet:AccessToken"], Model.Data[StaticStrings.ContactUsPageDatMapCoordinatesKey], Model.Data[StaticStrings.ContactUsPageDataMapMarkerTextKey]);
                await JS.InvokeVoidAsync(JSInvokeMethodList.enablePopovers);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await CheckSubscriptionStatus();

            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.ContactUsPageDataJsonMemoryCacheKey, StaticStrings.ContactUsPageDataJsonFilePath);
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
            await PageDataService.SaveDataAsync(model, StaticStrings.ContactUsPageDataJsonMemoryCacheKey, StaticStrings.ContactUsPageDataJsonFilePath);
        }

        public async Task SubmitForm()
        {
            var currentWebsite = await WebsiteService.GetWebsiteByName(StateManager.SiteName);
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

            await ContactUsMessageService.CreateContactUsMessage(message);

            await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.ContactUsFormAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.ContactUsFormSubmitted);

            Input = new ContactUsMessageModel();
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
