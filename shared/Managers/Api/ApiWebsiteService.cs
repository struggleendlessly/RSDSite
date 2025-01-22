using shared.Data.Entities;
using shared.Interfaces.Api;

namespace shared.Managers.Api
{
    public class ApiWebsiteService : IApiWebsiteService
    {
        private readonly IApiService _apiService;

        public ApiWebsiteService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<Website>> GetAllAsync()
        {
            var endpoint = StaticStrings.Route_API_Website;

            return await _apiService.SendGetRequestAsync<List<Website>>(endpoint);
        }

        public async Task<Website> GetWebsiteAsync(string siteName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "siteName", siteName }
            };

            var endpoint = StaticStrings.Route_API_Website + "/getByName";

            return await _apiService.SendGetRequestAsync<Website>(endpoint, parameters);
        }

        public async Task<string> GetSiteDomainAsync(string siteName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "siteName", siteName }
            };

            var endpoint = StaticStrings.Route_API_Website + "/domain";

            return await _apiService.SendGetRequestAsync<string>(endpoint, parameters);
        }

        public async Task<Website> CreateAsync(Website website)
        {
            var endpoint = StaticStrings.Route_API_Website;

            return await _apiService.SendPostRequestAsync<Website, Website>(endpoint, website);
        }

        public async Task<Website> UpdateAsync(Website website)
        {
            var endpoint = StaticStrings.Route_API_Website;

            return await _apiService.SendPutRequestAsync<Website, Website>(endpoint, website);
        }
    }
}
