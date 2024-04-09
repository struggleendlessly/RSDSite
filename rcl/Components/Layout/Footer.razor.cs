using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;

using Newtonsoft.Json;

namespace rcl.Components.Layout
{
    public partial class Footer
    {
        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IMemoryCache MemoryCache { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> SocialNetworks { get; set; } = new List<ServiceItem>();

        public PageModel SocialNetworksModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            var key = string.Format(StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var blobName = string.Format(StaticStrings.AdminPageSettingsDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var serviceItemsKey = string.Format(StaticStrings.AdminPageSocialNetworksDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(serviceItemsKey, out List<ServiceItem> serviceItems))
            {
                var blobName = string.Format(StaticStrings.AdminPageSocialNetworksDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                serviceItems = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);

                MemoryCache.Set(serviceItemsKey, serviceItems);
            }

            SocialNetworks = serviceItems;
            SocialNetworksModel.Data = SocialNetworks
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();
        }

        public string GetPageUrl(string url)
        {
            return $"{StateManager.SiteName}/{StateManager.Lang}/{url}";
        }

        public string GetCurrentUrlWithLanguage(string language)
        {
            var currentUrl = NavigationManager.Uri;
            var siteAndLang = $"{StateManager.SiteName}/{StateManager.Lang}";
            var newSiteAndLang = $"{StateManager.SiteName}/{language}";

            if (currentUrl.Contains(siteAndLang))
            {
                currentUrl = currentUrl.Replace(siteAndLang, newSiteAndLang);
            }
            else
            {
                currentUrl += newSiteAndLang;
            }

            return currentUrl;
        }
    }
}
