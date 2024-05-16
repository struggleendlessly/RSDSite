using Microsoft.AspNetCore.Components;

using shared.Models;

namespace rcl.Components.Shared
{
    public partial class Popover
    {
        [Parameter]
        public PageModel PopoversModel { get; set; }

        [Parameter]
        public string PopoverKey { get; set; }
    }
}
