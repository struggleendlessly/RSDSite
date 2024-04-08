using Microsoft.AspNetCore.Components;

namespace web.Components.Pages
{
    public partial class Service
    {
        [Parameter]
        public string SiteName { get; set; }

        [Parameter]
        public string Lang {  get; set; }

        [Parameter]
        public string Key { get; set; }
    }
}
