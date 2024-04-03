using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using shared;
using shared.Interfaces;
using shared.Managers;
using shared.Models;

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
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        public string SiteName { get; set; } = string.Empty;

        public string SiteNameLower { get; set; } = string.Empty;

        private string[] Pages { get; set; } = StaticRoutesStrings.GetPagesRoutes();

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            SetSiteName(currentUrl);

            NavigationManager.LocationChanged += OnLocationChanged;

            SiteNameLower = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.DefaultSiteName : SiteName.ToLower();
            var key = string.Format(StaticStrings.AdminPageDataJsonMemoryCacheKey, SiteNameLower);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(SiteNameLower, StaticStrings.AdminPageSettingsDataJsonFilePath);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var menuKey = string.Format(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, SiteNameLower);
            if (!MemoryCache.TryGetValue(menuKey, out PageModel menuModel))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(SiteNameLower, StaticStrings.AdminPageSettingsMenuDataJsonFilePath);
                menuModel = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(menuKey, menuModel);
            }

            MenuModel = menuModel;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            SetSiteName(currentUrl);

            StateHasChanged();
        }

        private string HighlightActiveMenuItem(string url)
        {
            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }

            if (currentUrl != url)
            {
                return string.Empty;
            }

            return StaticHtmlStrings.СSSActiveClass;
        }

        private void SetSiteName(string baseRelativePath)
        {
            string[] parts = baseRelativePath.Split('/');

            if (Pages.Contains(parts[0])) 
            {  
                SiteName = string.Empty;
                StateManager.SiteName = SiteName;
            }
            else
            {
                SiteName = parts.Length > 0 ? parts[0] : string.Empty;
                StateManager.SiteName = SiteName;
            }          
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
