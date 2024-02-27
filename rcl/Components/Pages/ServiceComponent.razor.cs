using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Pages
{
    public partial class ServiceComponent
    {
        [Inject]
        IFileManager FileManager { get; set; }

        [Parameter]
        public string Key { get; set; } = string.Empty;

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        protected override async Task OnInitializedAsync()
        {
            ServiceItems = FileManager.ReadFromJsonFile<List<ServiceItem>>(StaticStrings.WwwRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
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

            FileManager.WriteToJsonFile(ServiceItems, StaticStrings.WwwRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);

            return true;
        }
    }
}
