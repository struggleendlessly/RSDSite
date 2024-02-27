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

        public PageModel Model { get; set; } = new PageModel();

        public List<ServiceItem> ServiceItems { get; set; } = new List<ServiceItem>();

        protected override async Task OnInitializedAsync()
        {
            Model = FileManager.ReadFromJsonFile<PageModel>(StaticStrings.WwwRootPath, StaticStrings.ServicesPageDataJsonFilePath);
            ServiceItems = FileManager.ReadFromJsonFile<List<ServiceItem>>(StaticStrings.WwwRootPath, StaticStrings.ServicesPageServicesListDataJsonFilePath);
        }

        public bool Save(PageModel model)
        {
            FileManager.WriteToJsonFile(model, StaticStrings.WwwRootPath, StaticStrings.ServicesPageDataJsonFilePath);

            return true;
        }
    }
}
