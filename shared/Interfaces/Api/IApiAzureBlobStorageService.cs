using shared.Models.API;

namespace shared.Interfaces.Api
{
    public interface IApiAzureBlobStorageService
    {
        Task<string> UploadFileAsync(UploadFileModel model);
    }
}
