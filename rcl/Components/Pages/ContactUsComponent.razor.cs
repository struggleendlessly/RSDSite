using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Pages
{
    public partial class ContactUsComponent
    {
        [Inject]
        IFileManager FileManager { get; set; }

        [Parameter]
        public string? SiteName { get; set; }

        public string JsonPath { get; set; } = string.Empty;

        public PageModel Model { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            JsonPath = string.IsNullOrWhiteSpace(SiteName) ? StaticStrings.ContactUsPageDataJsonFilePath : string.Format(StaticStrings.ContactUsPageWebsiteDataJsonFilePath, SiteName);
            Model = FileManager.ReadFromJsonFile<PageModel>(StaticStrings.WwwRootPath, JsonPath);
        }

        public bool Save(PageModel model)
        {
            FileManager.WriteToJsonFile(model, StaticStrings.WwwRootPath, JsonPath);

            return true;
        }
    }
}
