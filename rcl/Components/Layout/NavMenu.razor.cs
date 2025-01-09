using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

namespace rcl.Components.Layout
{
    public partial class NavMenu : IDisposable
    {
        private string? currentUrl;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        public PageModel SettingsModel { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        public PageModel ServiceItemsModel { get; set; } = new PageModel();

        public PageModel PortfolioItemsModel { get; set; } = new PageModel();

        public PageModel DocumentsItemsModel { get; set; } = new PageModel();

        public PageModel ServiceItemsUrlsModel { get; set; } = new PageModel();

        public PageModel PortfolioItemsUrlsModel { get; set; } = new PageModel();

        public PageModel DocumentsItemsUrlsModel { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public List<ServiceItem> PortfolioServiceItems { get; set; } = new List<ServiceItem>();

        public List<ServiceItem> DocumentsServiceItems { get; set; } = new List<ServiceItem>();

        protected override async Task OnInitializedAsync()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            //SetSiteName(currentUrl);

            NavigationManager.LocationChanged += OnLocationChanged;

            SettingsModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsDataJsonFilePath);
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
            MenuModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
            ServiceItems = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.ServicesPageServicesListDataJsonFilePath);

            ServiceItemsModel.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();

            ServiceItemsUrlsModel.Data = ServiceItems
                .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.UrlKeyEnding)))
                .ToDictionary();

            if (StateManager.SiteName == StaticStrings.MainSiteName)
            {
                PortfolioServiceItems = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey + StaticStrings.PortfolioPageKeyEnding, StateManager.SiteName, StateManager.Lang, StaticStrings.PortfolioPageServicesListDataJsonFilePath);
                
                PortfolioItemsModel.Data = PortfolioServiceItems
                    .SelectMany(x => x.ShortDesc)
                    .ToDictionary();

                PortfolioItemsUrlsModel.Data = PortfolioServiceItems
                    .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.UrlKeyEnding)))
                    .ToDictionary();

                DocumentsServiceItems = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.ServicesPageServicesListDataJsonMemoryCacheKey + StaticStrings.DocumentsPageKeyEnding, StateManager.SiteName, StateManager.Lang, StaticStrings.DocumentsPageServicesListDataJsonFilePath);

                DocumentsItemsModel.Data = DocumentsServiceItems
                    .SelectMany(x => x.ShortDesc)
                    .ToDictionary();

                DocumentsItemsUrlsModel.Data = DocumentsServiceItems
                    .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.UrlKeyEnding)))
                    .ToDictionary();
            }
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            //SetSiteName(currentUrl);

            StateHasChanged();
        }

        private string HighlightActiveMenuItem(string url, bool isBtn = false)
        {
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            if (!string.IsNullOrWhiteSpace(currentUrl))
            {
                url = StateManager.GetPageUrl(url);
            }

            if (currentUrl != url)
            {
                return StaticHtmlStrings.СSSActiveLinkDark;
            }
            var res = $"{StaticHtmlStrings.СSSActiveLinkSecondary} {StaticHtmlStrings.СSSTextBold}";

            if (isBtn)
            {
                res = $"{StaticHtmlStrings.СSSTextBold}";
            }
            return res;
        }

        //private void SetSiteName(string baseRelativePath)
        //{
        //    string[] parts = baseRelativePath.Split('/');

        //    StateManager.SiteName = parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0] : StaticStrings.DefaultSiteName;
        //    StateManager.Lang = parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]) ? parts[1] : StaticStrings.DefaultEnLang;
        //}

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
