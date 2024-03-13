using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Pages
{
    public partial class ServicesComponent
    {
        [Inject]
        IFileManager FileManager { get; set; }

        [Parameter]
        public string? SiteName { get; set; }

        public string JsonPath { get; set; } = string.Empty;

        public string ServiceItemsJsonPath { get; set; } = string.Empty;

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        protected override async Task OnInitializedAsync()
        {
            JsonPath = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.ServicesPageDataJsonFilePath : string.Format(StaticStrings.ServicesPageWebsiteDataJsonFilePath, SiteName);
            ServiceItemsJsonPath = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.ServicesPageServicesListDataJsonFilePath : string.Format(StaticStrings.ServicesPageWebsiteServicesListDataJsonFilePath, SiteName);
            Model = FileManager.ReadFromJsonFile<PageModel>(StaticStrings.WwwRootPath, JsonPath);
            ServiceItems = FileManager.ReadFromJsonFile<List<ServiceItem>>(StaticStrings.WwwRootPath, ServiceItemsJsonPath);
        }

        public bool Save(PageModel model)
        {
            FileManager.WriteToJsonFile(model, StaticStrings.WwwRootPath, JsonPath);

            return true;
        }
    }
}
