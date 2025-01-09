using System.Globalization;

using shared.Models;
using shared.Interfaces.Api;

namespace shared.Managers.Api
{
    public class ApiPageDataService : IApiPageDataService
    {
        private readonly IApiService _apiService;

        public ApiPageDataService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<T> GetDataAsync<T>(string memoryCacheKey, string siteName, CultureInfo lang, string filePath, string? blobContainerName = null)
        {
            var parameters = new Dictionary<string, string>
            {
                { "memoryCacheKey", memoryCacheKey },
                { "siteName", siteName },
                { "lang", lang.TwoLetterISOLanguageName },
                { "filePath", filePath }
            };

            if (!string.IsNullOrEmpty(blobContainerName))
            {
                parameters.Add("blobContainerName", blobContainerName);
            }

            string endpoint = typeof(T) == typeof(PageModel) ? "api/pageData/pageModel" :
                              typeof(T) == typeof(List<ServiceItem>) ? "api/pageData/serviceItems" :
                              throw new NotSupportedException($"The type {typeof(T)} is not supported.");

            return await _apiService.SendGetRequestAsync<T>(endpoint, parameters);
        }

        public async Task<T> SaveDataAsync<T>(T model, string memoryCacheKey, string siteName, CultureInfo lang, string filePath)
        {
            var parameters = new Dictionary<string, string>
            {
                { "memoryCacheKey", memoryCacheKey },
                { "siteName", siteName },
                { "lang", lang.TwoLetterISOLanguageName },
                { "filePath", filePath }
            };

            string endpoint = typeof(T) == typeof(PageModel) ? "api/pageData/savePageModel" :
                              typeof(T) == typeof(List<ServiceItem>) ? "api/pageData/saveServiceItems" :
                              throw new NotSupportedException($"The type {typeof(T)} is not supported.");

            return await _apiService.SendPostRequestAsync<T, T>(endpoint, model, parameters);
        }
    }
}
