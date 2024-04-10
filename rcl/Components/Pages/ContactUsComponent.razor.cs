using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;
using shared.Data.Entities;

using System.Text;
using Newtonsoft.Json;
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

        public PageModel Model { get; set; } = new PageModel();

        [SupplyParameterFromForm]
        private ContactUsMessageModel Input { get; set; } = new();

        DotNetObjectReference<ContactUsComponent>? dotNetHelper { get; set; }

        public string Message { get; set; } = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetHelper = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync(JSInvokeMethodList.dotNetHelpersSetDotNetHelper, dotNetHelper);
                await JS.InvokeVoidAsync(JSInvokeMethodList.leafletActivate, Configuration["Leaflet:AccessToken"], Model.Data[StaticStrings.ContactUsPageDatMapCoordinatesKey], Model.Data[StaticStrings.ContactUsPageDataMapMarkerTextKey]);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.ContactUsPageDataJsonMemoryCacheKey, StaticStrings.ContactUsPageDataJsonFilePath);
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
            var blobName = string.Format(StaticStrings.ContactUsPageDataJsonFilePath, StateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

            var key = string.Format(StaticStrings.ContactUsPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            MemoryCache.Remove(key);
        }

        public async Task SubmitForm()
        {
            if (StateManager.SiteName == StaticStrings.DefaultSiteName)
            {
                return;
            }

            var currentWebsite = await WebsiteService.GetWebsiteByName(StateManager.SiteName);

            var message = new ContactUsMessage
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                Details = Input.Details,
                WebsiteId = currentWebsite.Id
            };

            await ContactUsMessageService.CreateContactUsMessage(message);

            Message = "Form submitted successfully.";
            Input = new ContactUsMessageModel();
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
