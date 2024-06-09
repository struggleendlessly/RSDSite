using shared;
using shared.Models;
using shared.Interfaces;
using shared.Models.Stripe;

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
        IStripeService StripeService { get; set; }

        [Inject]
        IPageDataService PageDataService { get; set; }

        public PageModel LocalizationModel { get; set; } = new PageModel();

        List<StripeProductModel> ProductsWithPrices { get; set; } = new List<StripeProductModel>();

        protected override async Task OnInitializedAsync()
        {
            LocalizationModel = await PageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
            ProductsWithPrices = await StripeService.GetProductsWithPricesAsync();
        }
    }
}
