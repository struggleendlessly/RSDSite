using Microsoft.AspNetCore.Components;

using shared;
using shared.Interfaces;
using shared.Managers;
using shared.Models;

namespace web.Components.Pages
{
    public partial class ContactUs : ITinyMceEditable
    {
        [Inject]
        IWebHostEnvironment hostingEnvironment { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        protected override async Task OnInitializedAsync()
        {
            Model = JsonFileManager.ReadFromJsonFile<PageModel>(hostingEnvironment.WebRootPath, StaticStrings.ContactUsPageDataJsonFilePath);
        }

        public void Save()
        {
            JsonFileManager.WriteToJsonFile(Model, hostingEnvironment.WebRootPath, StaticStrings.ContactUsPageDataJsonFilePath);
        }
    }
}
