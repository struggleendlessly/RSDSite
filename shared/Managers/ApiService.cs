using System.Text;
using Newtonsoft.Json;
using shared.Interfaces;
using shared.ConfigurationOptions;
using Microsoft.Extensions.Options;

namespace shared.Managers
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiOptions _apiOptions;

        public ApiService(HttpClient httpClient, IOptions<ApiOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiOptions = apiOptions.Value;
        }

        public async Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(TRequest request, string endpoint)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var requestUri = _apiOptions.Url + endpoint;

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiOptions.AccessToken}");

            var response = await _httpClient.PostAsync(requestUri, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(jsonResponse);
            }
            else
            {
                return default;
            }
        }
    }
}
