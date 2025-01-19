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

        public async Task<Website> GetWebsiteByName(string siteName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "siteName", siteName }
            };

            var endpoint = StaticStrings.Route_API_Website + "/getByName";

            return await _apiService.SendGetRequestAsync<Website>(endpoint, parameters);
        }
    }
}
