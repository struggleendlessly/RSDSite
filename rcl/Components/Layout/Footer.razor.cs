using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Layout
{
    public partial class Footer
    {
        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel LocalizationModel { get; set; } = new PageModel();

        public List<ServiceItem> SocialNetworks { get; set; } = new List<ServiceItem>();

        public PageModel SocialNetworksModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model = await PageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, "main", "en", StaticStrings.AdminPageSettingsDataJsonFilePath);
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, "main", "en", StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);  
            SocialNetworks = await PageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.AdminPageSocialNetworksDataJsonMemoryCacheKey, "main", "en", StaticStrings.AdminPageSocialNetworksDataJsonFilePath);      
            
            SocialNetworksModel.Data = SocialNetworks
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();
        }
    }
}
