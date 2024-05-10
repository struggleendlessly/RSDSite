using System.Text;
using Newtonsoft.Json;
using shared.Interfaces;

namespace shared.Managers
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7215";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(TRequest request, string endpoint)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var requestUri = _apiUrl + endpoint;

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
