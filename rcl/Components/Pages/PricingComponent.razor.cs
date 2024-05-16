using Microsoft.AspNetCore.Components;

using shared.Interfaces;
using shared.Models.Stripe;

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

        List<StripeProductModel> ProductsWithPrices { get; set; } = new List<StripeProductModel>();

        protected override async Task OnInitializedAsync()
        {
            ProductsWithPrices = await StripeService.GetProductsWithPricesAsync();
        }
    }
}
