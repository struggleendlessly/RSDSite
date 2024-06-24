using System.Text;

using Newtonsoft.Json;

using shared.Models;
using shared.Helpers;
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

            if (model is PageModel pageModel)
            {
                SearchAndReplaceInPageModel(pageModel, "{{{container}}}", _stateManager.SiteName);
            }

            return model;
        }

        public async Task<string> GetStringDataAsync(string memoryCacheKey, string filePath, string? blobContainerName = null)
        {
            var key = string.Format(memoryCacheKey, _stateManager.SiteName, _stateManager.Lang);
            if (!_memoryCache.TryGetValue(key, out string data))
            {
                var containerName = blobContainerName ?? _stateManager.SiteName;
                var blobName = string.Format(filePath, _stateManager.Lang);
                var content = await _blobStorageManager.DownloadFile(containerName, blobName);
                data = content;

                _memoryCache.Set(key, data);
            }

            return data;
        }

        public async Task SaveDataAsync<T>(T model, string memoryCacheKey, string filePath)
        {
            T modelCopy = ObjectHelpers.DeepCopy(model);

            if (modelCopy is PageModel pageModelCopy)
            {
                SearchAndReplaceInPageModel(pageModelCopy, _stateManager.SiteName, "{{{container}}}");
            }

            var jsonModel = JsonConvert.SerializeObject(modelCopy);
            var blobName = string.Format(filePath, _stateManager.Lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await _blobStorageManager.UploadFile(_stateManager.SiteName, blobName, stream);

            var key = string.Format(memoryCacheKey, _stateManager.SiteName, _stateManager.Lang);
            _memoryCache.Remove(key);
        }

        private void SearchAndReplaceInPageModel(PageModel pageModel, string target, string replacement)
        {
            foreach (var key in pageModel.Data.Keys.ToList())
            {
                if (pageModel.Data[key].Contains(target))
                {
                    var newValue = pageModel.Data[key].Replace(target, replacement);
                    pageModel.Data[key] = newValue;
                }
            }
        }
    }
}
