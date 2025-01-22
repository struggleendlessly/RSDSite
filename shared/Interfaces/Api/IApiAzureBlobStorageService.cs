using shared.Models.API;

namespace shared.Interfaces.Api
{
    public interface IApiAzureBlobStorageService
    {
        Task<string> UploadFileAsync(UploadFileModel model);
        Task RenameContainerAsync(RenameContainerModel model);
        Task CreateSiteAsync(string siteName);
    }
}
