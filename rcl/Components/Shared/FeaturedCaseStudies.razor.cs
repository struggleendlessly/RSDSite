using shared.Models;
using shared.Interfaces;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Shared
{
    public partial class FeaturedCaseStudies
    {
        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        [Parameter]
        public PageModel? PopoversModel { get; set; } = null;

        [Parameter]
        public string? PopoverKey { get; set; } = null;

        [Inject]
        IStateManager StateManager { get; set; }
    }
}
