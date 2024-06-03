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
        public IOptions<StripeOptions> stripeOptions { get; set; }

        [Inject]
        IOptions<DomainValidationOptions> DomainValidationOptions { get; set; }

        [Inject]
        ICustomDomainService CustomDomainService { get; set; }

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

        public PageModel SettingsModel { get; set; } = new PageModel();

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

        public bool IsWebsiteCreating { get; set; } = false;
        public bool IsWebsiteRenaming { get; set; } = false;

        public bool IsWebsiteSubscriptionActive { get; set; } = false;
        public bool IsWebsiteCustomDomainSubscriptionActive { get; set; } = false;

        public string CustomDomainVerificationMessage { get; set; } = string.Empty;

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
            PopoversModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversDataJsonMemoryCacheKey, StaticStrings.PopoversDataJsonFilePath, StaticStrings.PopoversContainerName);
            SettingsModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
            ContactUsMessages = await ContactUsMessageService.GetContactUsMessages(StateManager.SiteName);

            SelectedSite = StateManager.SiteName;
            CustomDomain = await WebsiteService.GetSiteDomainAsync(StateManager.SiteName);

            if (!string.IsNullOrWhiteSpace(CustomDomain))
            {
                CustomDomainVerificationMessage = await CustomDomainService.CheckCustomDomainVerificationAsync();
            }
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
            IsWebsiteCreating = true;
            await Task.Delay(1);

            var existingWebsite = await WebsiteService.GetWebsiteByName(CreateSiteModel.Name);
            if (existingWebsite != null)
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminCreateSiteFormAlertId, StaticHtmlStrings.CSSAlertDanger, StaticStrings.AdminCreateOrRenameSiteFormDuplicateSiteName);
                return;
            }

            var newWebsite = new Website { Name = CreateSiteModel.Name };
            await WebsiteService.CreateWebsite(newWebsite, StateManager.UserId);

            Logger.LogInformation($"A website named {newWebsite.Name} has been created.");

            await SiteCreator.CreateSite(newWebsite.Name);

            StateManager.AddUserSite(newWebsite);

            await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminCreateSiteFormAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.AdminCreateSiteFormSuccessfullyCreated);

            CreateSiteModel = new CreateSiteModel();
            IsWebsiteCreating = false;
        }

        public void ChangeSite()
        {
            var newUrl = NavigationManager.Uri.Replace(StateManager.SiteName, SelectedSite);
            NavigationManager.NavigateTo(newUrl);
        }

        public async Task RenameSite(EditContext editContext)
        {
            IsWebsiteRenaming = true;
            await Task.Delay(1);

            var existingWebsite = await WebsiteService.GetWebsiteByName(RenameSiteModel.NewName);
            if (existingWebsite != null)
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminRenameSiteFormAlertId, StaticHtmlStrings.CSSAlertDanger, StaticStrings.AdminCreateOrRenameSiteFormDuplicateSiteName);
                return;
            }

            var website = await WebsiteService.GetWebsiteByName(RenameSiteModel.SiteName);
            website.Name = RenameSiteModel.NewName;

            await WebsiteService.UpdateAsync(website);

            StateManager.RenameUserSite(website.Id, RenameSiteModel.NewName);

            await BlobStorageManager.RenameContainerAsync(RenameSiteModel.SiteName, RenameSiteModel.NewName);

            if (StateManager.SiteName == RenameSiteModel.SiteName)
            {
                var newUrl = NavigationManager.Uri.Replace(RenameSiteModel.SiteName, RenameSiteModel.NewName);
                NavigationManager.NavigateTo(newUrl);
            }
            else
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminRenameSiteFormAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.AdminRenameSiteFormSuccessfullyRenamed);
                
                RenameSiteModel = new RenameSiteModel();
                IsWebsiteRenaming = false;
            }
        }

        public void ToggleCustomDomainEditMode()
        {
            IsCustomDomainEditing = !IsCustomDomainEditing;
        }

        public async Task SaveCustomDomainAsync()
        {
            IsCustomDomainSaving = true;

            await CustomDomainService.SaveCustomDomainAsync(CustomDomain);

            ToggleCustomDomainEditMode();
            CustomDomainVerificationMessage = await CustomDomainService.CheckCustomDomainVerificationAsync();

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
