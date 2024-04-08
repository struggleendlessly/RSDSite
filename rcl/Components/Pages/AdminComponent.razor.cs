using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;
using shared.Data.Entities;

using System.Text;
using Newtonsoft.Json;
using System.Text.Json;

namespace rcl.Components.Pages
{
    public partial class AdminComponent
    {
        [Inject]
        IJSRuntime JS { get; set; }

        [Inject]
        AzureBlobStorageManager BlobStorageManager { get; set; }

        [Inject]
        protected IMemoryCache MemoryCache { get; set; }

        [Inject]
        IContactUsMessageService ContactUsMessageService { get; set; }

        [Inject]
        IStateManager StateManager { get; set; }

        public PageModel Model { get; set; } = new PageModel();

        public PageModel MenuModel { get; set; } = new PageModel();

        public List<ContactUsMessage> ContactUsMessages { get; set; } = new List<ContactUsMessage>();

        DotNetObjectReference<AdminComponent>? dotNetHelper { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                dotNetHelper = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync(JSInvokeMethodList.dotNetHelpersSetDotNetHelper, dotNetHelper);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var key = string.Format(StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(key, out PageModel model))
            {
                var blobName = string.Format(StaticStrings.AdminPageSettingsDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                model = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(key, model);
            }

            Model = model;

            var menuKey = string.Format(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            if (!MemoryCache.TryGetValue(menuKey, out PageModel menuModel))
            {
                var blobName = string.Format(StaticStrings.AdminPageSettingsMenuDataJsonFilePath, StateManager.Lang);
                var jsonContent = await BlobStorageManager.DownloadFile(StateManager.SiteName, blobName);
                menuModel = JsonConvert.DeserializeObject<PageModel>(jsonContent);

                MemoryCache.Set(menuKey, menuModel);
            }

            MenuModel = menuModel;

            ContactUsMessages = await ContactUsMessageService.GetContactUsMessages(StateManager.SiteName);
        }

        [JSInvokable]
        public async Task<string> returnTinyMceImage(JsonElement image)
        {
            var content = image.GetRawText();
            var base64 = content.Replace("\"", "");
            byte[] bytes = Convert.FromBase64String(base64);
            var blobName = $"{StateManager.Lang}/images/{Guid.NewGuid()}.png";

            using (MemoryStream stream = new MemoryStream(bytes))
            return await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);
        }

        public async Task Save(PageModel model)
        {
            var jsonModel = JsonConvert.SerializeObject(model);
            var blobName = string.Format(StaticStrings.AdminPageSettingsDataJsonFilePath, StateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

            var key = string.Format(StaticStrings.AdminPageDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            MemoryCache.Remove(key);
        }

        public async Task SaveMenu(PageModel model)
        {
            var jsonModel = JsonConvert.SerializeObject(model);
            var blobName = string.Format(StaticStrings.AdminPageSettingsMenuDataJsonFilePath, StateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await BlobStorageManager.UploadFile(StateManager.SiteName, blobName, stream);

            var key = string.Format(StaticStrings.AdminPageSettingsMenuDataJsonMemoryCacheKey, StateManager.SiteName, StateManager.Lang);
            MemoryCache.Remove(key);
        }

        public void Dispose()
        {
            dotNetHelper?.Dispose();
        }
    }
}
