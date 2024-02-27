using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Interfaces;

namespace rcl.Components.Pages
{
    public partial class HomeComponent
    {
        [Inject]
        IFileManager FileManager { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model = FileManager.ReadFromJsonFile<PageModel>(StaticStrings.WwwRootPath, StaticStrings.HomePageDataJsonFilePath);
        }

        public bool Save(PageModel model)
        {
            FileManager.WriteToJsonFile(model, StaticStrings.WwwRootPath, StaticStrings.HomePageDataJsonFilePath);

            return true;
        }
    }
}
