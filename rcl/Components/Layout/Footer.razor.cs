using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

namespace rcl.Components.Layout
{
    public partial class Footer
    {
        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel LocalizationModel { get; set; } = new PageModel();

        public List<ServiceItem> SocialNetworks { get; set; } = new List<ServiceItem>();

        public PageModel SocialNetworksModel { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSettingsDataJsonFilePath);
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);  
            SocialNetworks = await ApiPageDataService.GetDataAsync<List<ServiceItem>>(StaticStrings.AdminPageSocialNetworksDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.AdminPageSocialNetworksDataJsonFilePath);      
            
            SocialNetworksModel.Data = SocialNetworks
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();
        }
    }
}
