using Microsoft.AspNetCore.Components;

using shared;
using shared.Managers;
using shared.Models;

namespace web.Components.Pages
{
    public partial class AboutUs
    {
        [Inject]
        IWebHostEnvironment hostingEnvironment { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model = JsonFileManager.ReadFromJsonFile<PageModel>(hostingEnvironment.WebRootPath, StaticStrings.AboutUsPageDataJsonFilePath);
        }

        public bool Save(PageModel model)
        {
            JsonFileManager.WriteToJsonFile(model, hostingEnvironment.WebRootPath, StaticStrings.AboutUsPageDataJsonFilePath);

            return true;
        }
    }
}
