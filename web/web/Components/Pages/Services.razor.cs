using Microsoft.AspNetCore.Components;

using shared;
using shared.Managers;
using shared.Models;

namespace web.Components.Pages
{
    public partial class Services
    {
        [Inject]
        IWebHostEnvironment HostingEnvironment { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        protected override async Task OnInitializedAsync()
        {
            Model = JsonFileManager.ReadFromJsonFile<PageModel>(HostingEnvironment.WebRootPath, StaticStrings.ServicesPageDataJsonFilePath);
            ServiceItems = JsonFileManager.ReadFromJsonFile<List<ServiceItem>>(HostingEnvironment.WebRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
        }

        public bool Save(PageModel model)
        {
            JsonFileManager.WriteToJsonFile(model, HostingEnvironment.WebRootPath, StaticStrings.ServicesPageDataJsonFilePath);

            return true;
        }
    }
}
