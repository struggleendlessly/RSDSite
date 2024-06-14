using shared.Models;
using shared.Interfaces;

using Microsoft.AspNetCore.Components;

namespace rcl.Components.Shared
{
    public partial class NavMenuDropdown
    {
        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        [Parameter]
        public PageModel ServiceItemsModel { get; set; } = new PageModel();

        [Parameter]
        public PageModel ServiceItemsUrlsModel { get; set; } = new PageModel();

        [Parameter]
        public string MenuItemText { get; set; } = string.Empty;

        [Parameter]
        public string MenuItemClass { get; set; } = string.Empty;

        [Parameter]
        public string PageUrl { get; set; } = string.Empty;

        [Inject]
        IStateManager StateManager { get; set; }
    }
}
