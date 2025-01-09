using shared.Interfaces.Api;

namespace shared.Managers.Api
{
    public class ApiSubscriptionService : IApiSubscriptionService
    {
        private readonly IApiService _apiService;

        public ApiSubscriptionService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<bool> IsWebsiteSubscriptionActiveAsync(string siteName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "siteName", siteName }
            };

            string endpoint = "api/subscription/website";

            return await _apiService.SendGetRequestAsync<bool>(endpoint, parameters);
        }

        public async Task<bool> IsCustomDomainSubscriptionActiveAsync(string siteName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "siteName", siteName }
            };

            string endpoint = "api/subscription/custom-domain";

            return await _apiService.SendGetRequestAsync<bool>(endpoint, parameters);
        }
    }
}
