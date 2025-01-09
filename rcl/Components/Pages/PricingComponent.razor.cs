using shared;
using shared.Models;
using shared.Interfaces;
using shared.Models.Stripe;
using shared.Interfaces.Api;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Pages
{
    public partial class PricingComponent
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        [Inject]
        IApiStripeService ApiStripeService { get; set; }

        [Inject]
        IApiPageDataService ApiPageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        List<StripeProductModel> ProductsWithPrices { get; set; } = new List<StripeProductModel>();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await ApiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StateManager.SiteName, StateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
            ProductsWithPrices = await ApiStripeService.GetProductsWithPricesAsync(StateManager.Lang);
        }
    }
}
