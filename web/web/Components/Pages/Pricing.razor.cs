using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

using shared.ConfigurationOptions;

namespace web.Components.Pages
{
    public partial class Pricing
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
