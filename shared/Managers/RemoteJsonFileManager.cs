using Newtonsoft.Json;
using shared.Interfaces;

namespace shared.Managers
{
    public class RemoteJsonFileManager : IFileManager
    {
        private readonly HttpClient _httpClient;

        public RemoteJsonFileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public T ReadFromJsonFile<T>(string webRootPath, string jsonPath)
        {
            var fileName = jsonPath.Replace("data/", "");
            var apiUrl = $"https://localhost:7101/api/file/{fileName}";

            HttpResponseMessage response = _httpClient.GetAsync(apiUrl).Result;

            response.EnsureSuccessStatusCode();

            string content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(content);
        }

        public void WriteToJsonFile<T>(T data, string webRootPath, string jsonPath)
        {
            throw new NotImplementedException();
        }
    }
}
