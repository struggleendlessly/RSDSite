using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Routing;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Layout
{
    public partial class NavMenu : IDisposable
    {
        private string? currentUrl;

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            SetSiteName(currentUrl);

            NavigationManager.LocationChanged += OnLocationChanged;

            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsDataJsonFilePath);
            MenuModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
        }

        public string GetPageUrl(string url)
        {
            return $"{StateManager.SiteName}/{StateManager.Lang}/{url}";
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            SetSiteName(currentUrl);

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
                url = GetPageUrl(url);
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

        private void SetSiteName(string baseRelativePath)
        {
            string[] parts = baseRelativePath.Split('/');

            StateManager.SiteName = parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0] : StaticStrings.DefaultSiteName;
            StateManager.Lang = parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]) ? parts[1] : StaticStrings.DefaultEnLang;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
