using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using shared;
using shared.Interfaces;
using shared.Managers;
using shared.Models;

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

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> SocialNetworks { get; set; } = new List<ServiceItem>();

        public PageModel SocialNetworksModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            var key = string.Format(StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, StaticStrings.AdminPageSettingsDataJsonFilePath);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var serviceItemsKey = string.Format(StaticStrings.AdminPageSocialNetworksDataJsonMemoryCacheKey, StateManager.SiteName);
            if (!MemoryCache.TryGetValue(serviceItemsKey, out List<ServiceItem> serviceItems))
            {
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, StaticStrings.AdminPageSocialNetworksDataJsonFilePath);
                serviceItems = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);

                MemoryCache.Set(serviceItemsKey, serviceItems);
            }

            SocialNetworks = serviceItems;
            SocialNetworksModel.Data = SocialNetworks
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();
        }
    }
}
