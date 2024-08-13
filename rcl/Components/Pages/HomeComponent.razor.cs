using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Emails;
using shared.Models;
using shared.Managers;
using shared.Interfaces;

using System.Text.Json;
using shared.Interfaces.Api;

namespace rcl.Components.Pages
{
    public partial class HomeComponent : IDisposable
    {
        [Inject]
        EmailService EmailService { get; set; }
        [Inject]
        EmailSenders EmailSenders { get; set; }
        
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

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

        public PageModel LocalizationModel { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        DotNetObjectReference<HomeComponent>? dotNetHelper { get; set; }

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
            //var emailModel = new EmailModel
            //{
            //    Subject = "Hello from the Home Page",
            //    HtmlContent = "This is a test email from the Home Page",
            //    Recipient = "struggleendlessly@hotmail.com",
            //    Sender = EmailSenders.DoNotReply
            //};

            //await EmailService.Send(emailModel);

            await CheckSubscriptionStatus();

            Model = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.HomePageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.HomePageDataJsonFilePath);
            PopoversModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.PopoversDataJsonFilePath, StaticStrings.PopoversContainerName);

            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

            var serviceItems = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.ServicesPageServicesListDataJsonFilePath);
            ServiceItems = serviceItems.Take(4).ToList();
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
            //await PageDataService.SaveDataAsync(model, StaticStrings.HomePageDataJsonMemoryCacheKey, StaticStrings.HomePageDataJsonFilePath);
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
