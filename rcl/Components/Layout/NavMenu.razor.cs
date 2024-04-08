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

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            SetSiteName(currentUrl);

            NavigationManager.LocationChanged += OnLocationChanged;

            var key = string.Format(StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var blobName = string.Format(StaticStrings.AdminPageSettingsDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var menuKey = string.Format(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(menuKey, out PageModel menuModel))
            {
                var blobName = string.Format(StaticStrings.AdminPageSettingsMenuDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                menuModel = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(menuKey, menuModel);
            }

            MenuModel = menuModel;
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

        private string HighlightActiveMenuItem(string url)
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
                return string.Empty;
            }

            return StaticHtmlStrings.СSSActiveClass;
        }

        private void SetSiteName(string baseRelativePath)
        {
            string[] parts = baseRelativePath.Split('/');

            StateManager.SiteName = parts.Length >= 1 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0] : StaticStrings.DefaultSiteName;
            StateManager.Lang = parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]) ? parts[1] : StaticStrings.DefaultLang;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}
