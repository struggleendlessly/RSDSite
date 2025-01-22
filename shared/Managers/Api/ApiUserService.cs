using shared.Data.Entities;
using shared.Interfaces.Api;

namespace shared.Managers.Api
{
    public class ApiUserService : IApiUserService
    {
        private readonly IApiService _apiService;

        public ApiUserService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<User> GetOrCreateAsync()
        {
            string endpoint = StaticStrings.Route_API_User;

            return await _apiService.SendGetRequestAsync<User>(endpoint);
        }
    }
}
