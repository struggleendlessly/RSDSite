using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Forms;

using shared;
using shared.Data;
using shared.Models;
using shared.Managers;
using shared.Interfaces;
using shared.Models.API;
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
        
        [Inject]
        ApplicationDbContext context { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        ISubscriptionService SubscriptionService { get; set; }

        [Inject]
        IApiService ApiService { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

        public List<ContactUsMessage> ContactUsMessages { get; set; } = new List<ContactUsMessage>();

        DotNetObjectReference<AdminComponent>? dotNetHelper { get; set; }

        [SupplyParameterFromForm]
        public CreateSiteModel CreateSiteModel { get; set; } = new();

        [SupplyParameterFromForm]
        public RenameSiteModel RenameSiteModel { get; set; } = new();

        public string SelectedSite { get; set; }

        public string CustomDomain { get; set; }

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

            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
            MenuModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
            PopoversModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversDataJsonMemoryCacheKey, StaticStrings.PopoversDataJsonFilePath);
            ContactUsMessages = await ContactUsMessageService.GetContactUsMessages(StateManager.SiteName);

            SelectedSite = StateManager.SiteName;
            //CustomDomain = await WebsiteService.GetSiteDomainAsync(StateManager.SiteName);
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
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminCreateSiteFormAlertId, StaticHtmlStrings.CSSAlertDanger, StaticStrings.AdminCreateOrRenameSiteFormDuplicateSiteName);
                return;
            }

            var user = context.Users.Where(s => s.Id == StateManager.UserId).FirstOrDefault();
            var newWebsite = new Website { User = user, Name = CreateSiteModel.Name };
            await WebsiteService.CreateWebsite(newWebsite);

            Logger.LogInformation($"A website named {newWebsite.Name} has been created.");

            await SiteCreator.CreateSite(newWebsite.Name);

            StateManager.UserSites.Add(newWebsite.Name);

            await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminCreateSiteFormAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.AdminCreateSiteFormSuccessfullyCreated);
            CreateSiteModel = new CreateSiteModel();
        }

        public void ChangeSite()
        {
            var newUrl = NavigationManager.Uri.Replace(StateManager.SiteName, SelectedSite);
            NavigationManager.NavigateTo(newUrl);
        }

        public async Task RenameSite(EditContext editContext)
        {
            var existingWebsite = await WebsiteService.GetWebsiteByName(RenameSiteModel.NewName);
            if (existingWebsite != null)
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminRenameSiteFormAlertId, StaticHtmlStrings.CSSAlertDanger, StaticStrings.AdminCreateOrRenameSiteFormDuplicateSiteName);
                return;
            }

            var website = await WebsiteService.GetWebsiteByName(RenameSiteModel.SiteName);
            website.Name = RenameSiteModel.NewName;

            var result = await WebsiteService.UpdateAsync(website);

            for (int i = 0; i < StateManager.UserSites.Count; i++)
            {
                if (StateManager.UserSites[i] == RenameSiteModel.SiteName)
                {
                    StateManager.UserSites[i] = RenameSiteModel.NewName;
                }
            }

            await BlobStorageManager.RenameContainerAsync(RenameSiteModel.SiteName, RenameSiteModel.NewName);

            await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminRenameSiteFormAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.AdminRenameSiteFormSuccessfullyRenamed);
            RenameSiteModel = new RenameSiteModel();
        }

        public async Task SaveCustomDomainAsync()
        {
            //await WebsiteService.UpdateSiteDomainAsync(StateManager.SiteName, CustomDomain);

            var request = new RunPowerShellScriptModel
            {
                ScriptName = StaticStrings.PowerShellAzureAddCustomDomainScript,
                Parameters = new Dictionary<string, string>
                {
                    { StaticStrings.AzureWebAppName, StaticStrings.AzureDevWebAppNameValue },
                    { StaticStrings.AzureWebAppResourceGroup, StaticStrings.AzureDevWebAppResourceGroupValue },
                    { StaticStrings.AzureCustomDomain, CustomDomain }
                }
            };
            
            await ApiService.SendPostRequestAsync<RunPowerShellScriptModel, RunPowerShellScriptResponseModel>(request, StaticRoutesStrings.APIRunPowerShellScriptRoute);
        }

        public async Task CheckSubscriptionStatus()
        {
            var isSubscriptionActive = await SubscriptionService.IsWebsiteSubscriptionActive();
            if (!isSubscriptionActive)
            {
                NavigationManager.NavigateTo(StateManager.GetPageUrl(StaticRoutesStrings.SubscriptionErrorUrl));
            }
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
