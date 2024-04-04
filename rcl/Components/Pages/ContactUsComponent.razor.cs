using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Data.Entities;

using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using shared.Interfaces;
using Microsoft.SqlServer.Server;

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

        [Parameter]
        public string? SiteName { get; set; }

        public string SiteNameLower { get; set; } = string.Empty;

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
            SiteNameLower = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.DefaultSiteName : SiteName.ToLower();
            var key = string.Format(StaticStrings.ContactUsPageDataJsonMemoryCacheKey, SiteNameLower);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(SiteNameLower, StaticStrings.ContactUsPageDataJsonFilePath);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;
        }

        [JSInvokable]
        public async Task<string> returnTinyMceImage(JsonElement image)
        {
            var content = image.GetRawText();
            var base64 = content.Replace("\"", "");
            byte[] bytes = Convert.FromBase64String(base64);
            var blobName = $"images/{Guid.NewGuid()}.png";

            using (MemoryStream stream = new MemoryStream(bytes))
            return await BlobStorageManager.UploadFile(SiteNameLower, blobName, stream);
        }

        public async Task Save(PageModel model)
        {
            var jsonModel = JsonConvert.SerializeObject(model);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(SiteNameLower, StaticStrings.ContactUsPageDataJsonFilePath, stream);

            var key = string.Format(StaticStrings.ContactUsPageDataJsonMemoryCacheKey, SiteNameLower);
            MemoryCache.Remove(key);
        }

        public async Task SubmitForm()
        {
            if (SiteNameLower == StaticStrings.DefaultSiteName)
            {
                return;
            }

            var currentWebsite = await WebsiteService.GetWebsiteByName(SiteNameLower);

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
