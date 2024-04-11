using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Forms;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;
using shared.Data.Entities;

using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class AdminComponent
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Inject]
        IContactUsMessageService ContactUsMessageService { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        [Inject]
        IWebsiteService WebsiteService { get; set; }

        [Inject]
        ILogger<AdminComponent> Logger { get; set; }

        [Inject]
        ISiteCreator SiteCreator { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        public List<ContactUsMessage> ContactUsMessages { get; set; } = new List<ContactUsMessage>();

        DotNetObjectReference<AdminComponent>? dotNetHelper { get; set; }

        [SupplyParameterFromForm]
        public CreateSiteModel CreateSiteModel { get; set; } = new();

        public string CreateSiteMessage { get; set; } = string.Empty;

        private IEnumerable<IdentityError>? identityErrors;

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
            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
            MenuModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
            ContactUsMessages = await ContactUsMessageService.GetContactUsMessages(StateManager.SiteName);
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
            await PageDataService.SaveDataAsync(model, StaticStrings.AdminPageDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
        }

        public async Task SaveMenu(PageModel model)
        {
            await PageDataService.SaveDataAsync(model, StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
        }

        public async Task CreateSite(EditContext editContext)
        {
            var existingWebsite = await WebsiteService.GetWebsiteByName(CreateSiteModel.Name);
            if (existingWebsite != null)
            {
                identityErrors = new List<IdentityError>
                {
                    new IdentityError
                    {
                        Code = "DuplicateSiteName",
                        Description = "The site name is already taken. Please choose a different one."
                    }
                };

                return;
            }

            var newWebsite = new Website { UserId = StateManager.UserId, Name = CreateSiteModel.Name };
            await WebsiteService.CreateWebsite(newWebsite);

            Logger.LogInformation($"A website named {newWebsite.Name} has been created.");

            await SiteCreator.CreateSite(newWebsite.Name);

            StateManager.UserSites.Add(newWebsite.Name);

            CreateSiteMessage = "The site was successfully created";
            CreateSiteModel = new CreateSiteModel();
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
