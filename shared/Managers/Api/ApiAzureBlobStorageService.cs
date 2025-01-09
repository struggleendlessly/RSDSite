using shared.Models.API;
using shared.Interfaces.Api;

namespace shared.Managers.Api
{
    public class ApiAzureBlobStorageService : IApiAzureBlobStorageService
    {
        private readonly IApiService _apiService;

        public ApiAzureBlobStorageService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string> UploadFileAsync(UploadFileModel model)
        {
            var endpoint = StaticStrings.Route_API_File + "/upload";

            return await _apiService.SendPostRequestAsync<UploadFileModel, string>(endpoint, model);
        }
    }
}
