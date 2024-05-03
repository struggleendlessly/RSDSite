using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;

using shared;
using shared.Emails;
using shared.Interfaces;
using shared.Managers;
using shared.Models;

using System.Text.Json;

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

        public PageModel Model { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

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

            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.HomePageDataJsonMemoryCacheKey, StaticStrings.HomePageDataJsonFilePath);
            PopoversModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversDataJsonMemoryCacheKey, StaticStrings.PopoversDataJsonFilePath);

            var serviceItems = await PageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StaticStrings.ServicesPageServicesListDataJsonFilePath);
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
            await PageDataService.SaveDataAsync(model, StaticStrings.HomePageDataJsonMemoryCacheKey, StaticStrings.HomePageDataJsonFilePath);
        }

        public async Task CheckSubscriptionStatus()
        {
            var isSubscriptionActive = await SubscriptionService.IsWebsiteSubscriptionActive();
            if (!isSubscriptionActive)
            {
                NavigationManager.NavigateTo(StaticRoutesStrings.SubscriptionErrorPageRoute);
            }
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
