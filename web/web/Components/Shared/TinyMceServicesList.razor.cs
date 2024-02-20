using Microsoft.AspNetCore.Components;

using shared;
using shared.Extensions;
using shared.Managers;
using shared.Models;

namespace web.Components.Shared
{
    public partial class TinyMceServicesList
    {
        [Inject]
        IWebHostEnvironment HostingEnvironment { get; set; }

        [Parameter]
        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        public PageModel Model { get; set; } = new PageModel();

        public PageModel ModelUrls { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model.Data = ServiceItems
                .SelectMany(x => x.ShortDesc)
                .ToDictionary();

            ModelUrls.Data = ServiceItems
                .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.ServicesUrlKeyEnding)))
                .ToDictionary();
        }

        public bool SaveContent(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var shortDesc in serviceItem.ShortDesc.ToList())
                {
                    if (model.Data.TryGetValue(shortDesc.Key, out var modelData) && modelData != shortDesc.Value)
                    {
                        serviceItem.ShortDesc[shortDesc.Key] = modelData;
                    }
                }
            }

            JsonFileManager.WriteToJsonFile(ServiceItems, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);

            return true;
        }

        public bool SaveUrl(PageModel model)
        {
            foreach (var serviceItem in ServiceItems)
            {
                foreach (var longDesc in serviceItem.LongDesc.Where(x => x.Key.Contains(StaticStrings.ServicesUrlKeyEnding)).ToList())
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

        public void Add(string key)
        {
            var dateTime = DateTime.Now;
            var serviceItemKey = string.Format(StaticHtmlStrings.ServicesListServiceShortDescDefaultKey, dateTime.ToString("mm"), dateTime.ToString("ss"));
            var serviceItemShortDescValue = StaticHtmlStrings.ServicesListServiceShortDescDefaultValue;

            Model.Data.AddAfter(key, serviceItemKey, serviceItemShortDescValue);

            var serviceItem = new ServiceItem();
            serviceItem.ShortDesc = new Dictionary<string, string> { { serviceItemKey, serviceItemShortDescValue } };
            serviceItem.LongDesc = new Dictionary<string, string> 
            { 
                { serviceItemKey,  StaticHtmlStrings.ServicesListServiceLongDescDefaultValue },
                { serviceItemKey + StaticStrings.ServicesUrlKeyEnding, serviceItemKey }
            };

            var index = ServiceItems.FindIndex(x => x.ShortDesc.ContainsKey(key));
            ServiceItems.Insert(index + 1, serviceItem);

            JsonFileManager.WriteToJsonFile(ServiceItems, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
        }
    }
}
