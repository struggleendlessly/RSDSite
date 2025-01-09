using shared.Data.Entities;
using shared.Interfaces.Api;

namespace shared.Managers.Api
{
    public class ApiContactUsMessageService : IApiContactUsMessageService
    {
        private readonly IApiService _apiService;

        public ApiContactUsMessageService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<ContactUsMessage>> GetAllAsync(string siteName)
        {
            var parameters = new Dictionary<string, string>
            {
                { "siteName", siteName }
            };

            string endpoint = StaticStrings.Route_API_ContactUsMessage;

            return await _apiService.SendGetRequestAsync<List<ContactUsMessage>>(endpoint, parameters);
        }

        public async Task<ContactUsMessage> CreateAsync(ContactUsMessage model)
        {
            var endpoint = StaticStrings.Route_API_ContactUsMessage;

            return await _apiService.SendPostRequestAsync<ContactUsMessage, ContactUsMessage>(endpoint, model);
        }
    }
}
