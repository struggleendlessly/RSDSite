using Microsoft.JSInterop;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Authorization;

using shared;
using shared.Models;
using shared.Models.API;
using shared.Interfaces;
using shared.Data.Entities;
using shared.Interfaces.Api;
using shared.ConfigurationOptions;

using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class AdminComponent
    {
        [Inject]
        IJSRuntime JS { get; set; } = default!;

        [Inject]
        IApiAzureBlobStorageService ApiAzureBlobStorageService { get; set; } = default!;

        [Inject]
        IApiContactUsMessageService ApiContactUsMessageService { get; set; } = default!;

        [Inject]
        IStateManager StateManager { get; set; } = default!;

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; } = default!;

        [Inject]
        IApiWebsiteService ApiWebsiteService { get; set; } = default!;

        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        IApiSubscriptionService ApiSubscriptionService { get; set; } = default!;

        [Inject]
        public IOptions<StripeOptions> StripeOptions { get; set; } = default!;

        [Inject]
        IOptions<DomainValidationOptions> DomainValidationOptions { get; set; } = default!;

        [Inject]
        ICustomDomainService CustomDomainService { get; set; } = default!;

        [CascadingParameter]
        Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        public PageModel PopoversModel { get; set; } = new PageModel();

        public PageModel SettingsModel { get; set; } = new PageModel();

        public PageModel LocalizationModel {  get; set; } = new PageModel();

        public List<ContactUsMessage> ContactUsMessages { get; set; } = new List<ContactUsMessage>();

        DotNetObjectReference<AdminComponent>? dotNetHelper { get; set; }

        [SupplyParameterFromForm]
        public CreateSiteModel CreateSiteModel { get; set; } = new();

        [SupplyParameterFromForm]
        public RenameSiteModel RenameSiteModel { get; set; } = new();

        public string SelectedSite { get; set; } = default!;

        public string CustomDomain { get; set; } = default!;
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

            Model = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsDataJsonFilePath);
            MenuModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
            PopoversModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.PopoversMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.PopoversDataJsonFilePath, StaticStrings.PopoversContainerName);
            SettingsModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsDataJsonFilePath);
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
            ContactUsMessages = await ApiContactUsMessageService.GetAllAsync(StateManager.SiteName);

            SelectedSite = StateManager.SiteName;
            CustomDomain = await ApiWebsiteService.GetSiteDomainAsync(StateManager.SiteName);

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

            var uploadFileModel = new UploadFileModel()
            {
                SiteName = StateManager.SiteName,
                BlobName = blobName,
                FileData = bytes
            };

            var result = await ApiAzureBlobStorageService.UploadFileAsync(uploadFileModel);
            return result;
        }

        public async Task Save(PageModel model)
        {
            await ApiPageDataService.SaveDataAsync(model, StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsDataJsonFilePath);
        }

        public async Task SaveMenu(PageModel model)
        {
            await ApiPageDataService.SaveDataAsync(model, StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
        }

        public async Task CreateSite(EditContext editContext)
        {
            IsWebsiteCreating = true;
            await Task.Delay(1);

            var existingWebsite = await ApiWebsiteService.GetWebsiteAsync(CreateSiteModel.Name);
            if (existingWebsite != null)
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminCreateSiteFormAlertId, StaticHtmlStrings.CSSAlertDanger, LocalizationModel.Data[StaticStrings.Localization_Admin_Settings_CreateOrRenameSite_DuplicateSiteName_Message_Key]);
                return;
            }

            var newWebsite = new Website { Name = CreateSiteModel.Name };
            await ApiWebsiteService.CreateAsync(newWebsite);

            StateManager.AddUserSite(newWebsite);

            await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminCreateSiteFormAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.Localization_Admin_Settings_CreateSite_Success_Message_Key);

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

            var existingWebsite = await ApiWebsiteService.GetWebsiteAsync(RenameSiteModel.NewName);
            if (existingWebsite != null)
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminRenameSiteFormAlertId, StaticHtmlStrings.CSSAlertDanger, StaticStrings.Localization_Admin_Settings_CreateOrRenameSite_DuplicateSiteName_Message_Key);
                return;
            }

            var website = await ApiWebsiteService.GetWebsiteAsync(RenameSiteModel.SiteName);
            website.Name = RenameSiteModel.NewName;

            await ApiWebsiteService.UpdateAsync(website);

            StateManager.RenameUserSite(website.Id, RenameSiteModel.NewName);

            var renameContainerModel = new RenameContainerModel
            {
                SourceContainerName = RenameSiteModel.SiteName,
                DestinationContainerName = RenameSiteModel.NewName
            };

            await ApiAzureBlobStorageService.RenameContainerAsync(renameContainerModel);

            if (StateManager.SiteName == RenameSiteModel.SiteName)
            {
                var newUrl = NavigationManager.Uri.Replace(RenameSiteModel.SiteName, RenameSiteModel.NewName);
                NavigationManager.NavigateTo(newUrl);
            }
            else
            {
                await JS.InvokeVoidAsync(JSInvokeMethodList.showAndHideAlert, StaticHtmlStrings.AdminRenameSiteFormAlertId, StaticHtmlStrings.CSSAlertSuccess, StaticStrings.Localization_Admin_Settings_RenameSite_Success_Message_Key);
                
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
            IsWebsiteSubscriptionActive = await ApiSubscriptionService.IsWebsiteSubscriptionActiveAsync(StateManager.SiteName);
            IsWebsiteCustomDomainSubscriptionActive = await ApiSubscriptionService.IsCustomDomainSubscriptionActiveAsync(StateManager.SiteName);

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
