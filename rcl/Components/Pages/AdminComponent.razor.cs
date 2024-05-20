using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;
using shared.Models.API;
using shared.Data.Entities;
using shared.ConfigurationOptions;

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
        NavigationManager NavigationManager { get; set; }

        [Inject]
        ISubscriptionService SubscriptionService { get; set; }

        [Inject]
        IApiService ApiService { get; set; }

        [Inject]
        public IOptions<StripeOptions> stripeOptions { get; set; }

        [Inject]
        IOptions<AzureOptions> AzureOptions { get; set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

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
        public bool IsCustomDomainEditing { get; set; } = false;
        public bool IsCustomDomainSaving { get; set; } = false;

        public bool IsWebsiteSubscriptionActive { get; set; } = false;
        public bool IsWebsiteCustomDomainSubscriptionActive { get; set; } = false;

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
            CustomDomain = await WebsiteService.GetSiteDomainAsync(StateManager.SiteName);
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

            var newWebsite = new Website { UserId = StateManager.UserId, Name = CreateSiteModel.Name };
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

        public void ToggleCustomDomainEditMode()
        {
            IsCustomDomainEditing = !IsCustomDomainEditing;
        }

        public async Task SaveCustomDomainAsync()
        {
            IsCustomDomainSaving = true;
            await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminSaveCustomDomainAlertId, StaticHtmlStrings.CSSAlertInfo, StaticStrings.AdminAddCustomDomainInProgress);

            var request = new RunPowerShellScriptModel
            {
                ScriptName = StaticStrings.PowerShellAzureAddCustomDomainScript,
                Parameters = new Dictionary<string, string>
                {
                    { StaticStrings.AzureWebAppName, AzureOptions.Value.WebAppName },
                    { StaticStrings.AzureWebAppResourceGroup, AzureOptions.Value.WebAppResourceGroup },
                    { StaticStrings.AzureCustomDomain, CustomDomain }
                }
            };

            var result = await ApiService.SendPostRequestAsync<RunPowerShellScriptModel, RunPowerShellScriptResponseModel>(request, StaticRoutesStrings.APIRunPowerShellScriptRoute);
            if (result.Success)
            {
                await WebsiteService.UpdateSiteDomainAsync(StateManager.SiteName, CustomDomain);
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminSaveCustomDomainAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.AdminAddCustomDomainSuccess);
                ToggleCustomDomainEditMode();
            }
            else
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminSaveCustomDomainAlertId, StaticHtmlStrings.CSSAlertDanger, StaticStrings.AdminAddCustomDomainFailed);
            }

            IsCustomDomainSaving = false;
        }

        public async Task CheckSubscriptionStatus()
        {
            IsWebsiteSubscriptionActive = await SubscriptionService.IsWebsiteSubscriptionActiveAsync();
            IsWebsiteCustomDomainSubscriptionActive = await SubscriptionService.IsCustomDomainSubscriptionActiveAsync();

            var authenticationState = await AuthenticationStateTask;
            if (!authenticationState.User.Identity.IsAuthenticated && !IsWebsiteSubscriptionActive)
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
