using Microsoft.AspNetCore.Components;

using shared;
using shared.Interfaces;
using shared.Managers;
using shared.Models;

namespace web.Components.Shared
{
    public partial class TinyMceServicesList : ITinyMceEditable
    {
        [Inject]
        IWebHostEnvironment HostingEnvironment { get; set; }

        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        [Parameter]
        public ITinyMceEditable Parent { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();
        }

        public void Save()
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var shortDesc in serviceItem.ShortDesc.ToList())
                {
                    if (Model.Data.TryGetValue(shortDesc.Key, out var modelData) && modelData != shortDesc.Value)
                    {
                        serviceItem.ShortDesc[shortDesc.Key] = modelData;
                    }
                }
            }

            JsonFileManager.WriteToJsonFile(ServiceItems, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
        }
    }
}
