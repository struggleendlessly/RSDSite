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
            var endpoint = StaticStrings.Route_API_AzureBlobStorage + "/upload";

            return await _apiService.SendPostRequestAsync<UploadFileModel, string>(endpoint, model);
        }

        public async Task RenameContainerAsync(RenameContainerModel model)
        {
            var endpoint = StaticStrings.Route_API_AzureBlobStorage + "/rename-container";

            await _apiService.SendPostRequestAsync<RenameContainerModel, Task>(endpoint, model);
        }

        public async Task CreateSiteAsync(string siteName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "siteName", siteName }
            };

            string endpoint = StaticStrings.Route_API_AzureBlobStorage + "/create-site";

            await _apiService.SendGetRequestAsync<Task>(endpoint, parameters);
        }
    }
}
