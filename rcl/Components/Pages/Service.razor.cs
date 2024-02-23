using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Managers;

namespace rcl.Components.Pages
{
    public partial class Service
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

        public bool Save(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var longDesc in serviceItem.LongDesc.ToList())
                {
                    if (model.Data.TryGetValue(longDesc.Key, out var modelData) && modelData != longDesc.Value)
                    {
                        serviceItem.LongDesc[longDesc.Key] = modelData;
                    }
                }
            }

            JsonFileManager.WriteToJsonFile(ServiceItems, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);

            return true;
        }
    }
}
