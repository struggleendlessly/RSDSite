using shared.Models.API;
using shared.Interfaces.Api;

namespace shared.Managers.Api
{
    public class ApiCustomDomainService : IApiCustomDomainService
    {
        private readonly IApiService _apiService;

        public ApiCustomDomainService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string> CheckCustomDomainVerificationAsync(CheckCustomDomainVerificationModel model)
        {
            var endpoint = StaticStrings.Route_API_ContactUsMessage + "/check-verification";

            return await _apiService.SendPostRequestAsync<CheckCustomDomainVerificationModel, string>(endpoint, model);
        }

        public async Task SaveCustomDomainAsync(SaveCustomDomainModel model)
        {
            var endpoint = StaticStrings.Route_API_CustomDomain + "/save";

            await _apiService.SendPostRequestAsync<SaveCustomDomainModel, Task>(endpoint, model);
        }
    }
}
