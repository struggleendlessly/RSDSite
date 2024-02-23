using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components;

using shared;
using shared.Models;
using shared.Managers;

namespace rcl.Components.Pages
{
    public partial class Home
    {
        [Inject]
        IWebHostEnvironment hostingEnvironment { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model = JsonFileManager.ReadFromJsonFile<PageModel>(hostingEnvironment.WebRootPath, StaticStrings.HomePageDataJsonFilePath);
        }

        public bool Save(PageModel model)
        {
            JsonFileManager.WriteToJsonFile(model, hostingEnvironment.WebRootPath, StaticStrings.HomePageDataJsonFilePath);

            return true;
        }
    }
}
