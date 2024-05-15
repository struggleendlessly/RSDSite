using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;

using shared.ConfigurationOptions;

namespace rcl.Components.Pages
{
    public partial class PricingComponent
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang { get; set; }

        [Inject]
        public IOptions<StripeOptions> stripeOptions { get; set; }

        protected override async Task OnInitializedAsync()
        {

        }
    }
}
