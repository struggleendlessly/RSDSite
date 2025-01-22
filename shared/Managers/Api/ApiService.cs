using System.Text;

using Newtonsoft.Json;

using Microsoft.Extensions.Options;

using shared.Interfaces.Api;
using shared.ConfigurationOptions;

namespace shared.Managers.Api
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

        public async Task<T> SendGetRequestAsync<T>(string endpoint, Dictionary<string, string>? parameters = null)
        {
            var queryString = parameters != null
                ? "?" + string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))
                : string.Empty;

            var requestUri = $"{_apiOptions.Url}/{endpoint}{queryString}";

            var response = await _httpClient.GetAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            else
            {
                throw new HttpRequestException($"Request to {requestUri} failed with status code {response.StatusCode}");
            }
        }

        public async Task<TResult> SendPostRequestAsync<TRequest, TResult>(string endpoint, TRequest model, Dictionary<string, string>? parameters = null)
        {
            var queryString = parameters != null
                ? "?" + string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))
                : string.Empty;

            var requestUri = $"{_apiOptions.Url}/{endpoint}{queryString}";
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUri, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonResponse);
            }
            else
            {
                throw new HttpRequestException($"Request to {requestUri} failed with status code {response.StatusCode}");
            }
        }

        public async Task<TResult> SendPutRequestAsync<TRequest, TResult>(string endpoint, TRequest model, Dictionary<string, string>? parameters = null)
        {
            var queryString = parameters != null
                ? "?" + string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))
                : string.Empty;

            var requestUri = $"{_apiOptions.Url}/{endpoint}{queryString}";
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, content);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(jsonResponse);
            }
            else
            {
                throw new HttpRequestException($"Request to {requestUri} failed with status code {response.StatusCode}");
            }
        }
    }
}
