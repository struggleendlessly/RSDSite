using Microsoft.AspNetCore.Components;

using shared;
using shared.Interfaces;
using shared.Managers;
using shared.Models;

namespace web.Components.Pages
{
    public partial class Service : ITinyMceEditable
    {
        [Parameter]
        public string Key { get; set; } = string.Empty;

        [Inject]
        IWebHostEnvironment HostingEnvironment { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        protected override async Task OnInitializedAsync()
        {
            ServiceItems = JsonFileManager.ReadFromJsonFile<List<ServiceItem>>(HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
            Model.Data = ServiceItems
                .SelectMany(x => x.LongDesc)
                .Where(x => x.Key == Key)
                .ToDictionary();
        }

        public void Save()
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var longDesc in serviceItem.LongDesc.ToList())
                {
                    if (Model.Data.TryGetValue(longDesc.Key, out var modelData) && modelData != longDesc.Value)
                    {
                        serviceItem.LongDesc[longDesc.Key] = modelData;
                    }
                }
            }

            JsonFileManager.WriteToJsonFile(ServiceItems, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
        }
    }
}
