using System.Text;
using Newtonsoft.Json;
using shared.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace shared.Managers
{
    public class PageDataService : IPageDataService
    {
        private readonly AzureBlobStorageManager _blobStorageManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IStateManager _stateManager;

        public PageDataService(AzureBlobStorageManager blobStorageManager, IMemoryCache memoryCache, IStateManager stateManager)
        {
            _blobStorageManager = blobStorageManager;
            _memoryCache = memoryCache;
            _stateManager = stateManager;
        }

        public async Task<T> GetDataAsync<T>(string memoryCacheKey, string filePath, string? blobContainerName = null)
        {
            var key = string.Format(memoryCacheKey, _stateManager.SiteName, _stateManager.Lang);
            if (!_memoryCache.TryGetValue(key, out T model))
            {
                var containerName = blobContainerName ?? _stateManager.SiteName;
                var blobName = string.Format(filePath, _stateManager.Lang);
                var jsonContent = await _blobStorageManager.DownloadFile(containerName, blobName);
                model = JsonConvert.DeserializeObject<T>(jsonContent);

                _memoryCache.Set(key, model);
            }

            return model;
        }

        public async Task SaveDataAsync<T>(T model, string memoryCacheKey, string filePath)
        {
            var jsonModel = JsonConvert.SerializeObject(model);
            var blobName = string.Format(filePath, _stateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await _blobStorageManager.UploadFile(_stateManager.SiteName, blobName, stream);

            var key = string.Format(memoryCacheKey, _stateManager.SiteName, _stateManager.Lang);
            _memoryCache.Remove(key);
        }
    }
}
