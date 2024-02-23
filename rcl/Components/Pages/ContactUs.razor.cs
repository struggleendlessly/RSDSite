using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;

using shared;
using shared.Managers;
using shared.Models;

namespace rcl.Components.Pages
{
    public partial class ContactUs
    {
        [Inject]
        IWebHostEnvironment hostingEnvironment { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model = JsonFileManager.ReadFromJsonFile<PageModel>(hostingEnvironment.WebRootPath, StaticStrings.ContactUsPageDataJsonFilePath);
        }

        public bool Save(PageModel model)
        {
            JsonFileManager.WriteToJsonFile(model, hostingEnvironment.WebRootPath, StaticStrings.ContactUsPageDataJsonFilePath);

            return true;
        }
    }
}
