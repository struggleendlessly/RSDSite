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

        public void Remove(string key)
        {
            if (Model.Data.ContainsKey(key))
            {
                Model.Data.Remove(key);

                var serviceItem = ServiceItems.FirstOrDefault(x => x.ShortDesc.ContainsKey(key));
                if (serviceItem != null)
                {
                    ServiceItems.Remove(serviceItem);
                    JsonFileManager.WriteToJsonFile(ServiceItems, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
                }
            }
        }

        public void Add()
        {
            var dateTime = DateTime.Now;
            var key = $"Service_{dateTime.ToString("mm_ss")}";
            var value = "<h4>New service</h4><p>Some description for new service.</p>";

            Model.Data.Add(key, value);

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string> { { key, value } };
            serviceItem.LongDesc = new Dictionary<string, string> { { key, "LongDesc" } };

            ServiceItems.Add(serviceItem);

            JsonFileManager.WriteToJsonFile(ServiceItems, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
        }
    }
}
