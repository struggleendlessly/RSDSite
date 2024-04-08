using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;

using Newtonsoft.Json;

using shared;
using shared.Emails;
using shared.Interfaces;
using shared.Managers;
using shared.Models;

using System.Text;
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

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        DotNetObjectReference<HomeComponent>? dotNetHelper { get; set; }

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
            //var emailModel = new EmailModel
            //{
            //    Subject = "Hello from the Home Page",
            //    HtmlContent = "This is a test email from the Home Page",
            //    Recipient = "struggleendlessly@hotmail.com",
            //    Sender = EmailSenders.DoNotReply
            //};

            //await EmailService.Send(emailModel);

            var key = string.Format(StaticStrings.HomePageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var blobName = string.Format(StaticStrings.HomePageDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var serviceItemsKey = string.Format(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(serviceItemsKey, out List<ServiceItem> serviceItems))
            {
                var blobName = string.Format(StaticStrings.ServicesPageServicesListDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                serviceItems = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);
            }

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
            var jsonModel = JsonConvert.SerializeObject(model);
            var blobName = string.Format(StaticStrings.HomePageDataJsonFilePath, StateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

            var key = string.Format(StaticStrings.HomePageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            MemoryCache.Remove(key);
        }

        public string GetPageUrl(string url)
        {
            return $"{StateManager.SiteName}/{StateManager.Lang}/{url}";
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
