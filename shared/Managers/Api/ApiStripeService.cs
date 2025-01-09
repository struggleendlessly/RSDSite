using shared.Models.Stripe;
using shared.Interfaces.Api;

using System.Globalization;

namespace shared.Managers.Api
{
    public class ApiStripeService : IApiStripeService
    {
        private readonly IApiService _apiService;

        public ApiStripeService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<StripeProductModel>> GetProductsWithPricesAsync(CultureInfo lang)
        {
            var parameters = new Dictionary<string, string>
            {
                { "lang", lang.TwoLetterISOLanguageName }
            };

            string endpoint = "api/pricing";

            return await _apiService.SendGetRequestAsync<List<StripeProductModel>>(endpoint, parameters);
        }
    }
}
