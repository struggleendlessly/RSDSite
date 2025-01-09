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

        public PageDataService(AzureBlobStorageManager blobStorageManager, IMemoryCache memoryCache)
        {
            _blobStorageManager = blobStorageManager;
            _memoryCache = memoryCache;
        }

        public async Task<T> GetDataAsync<T>(string memoryCacheKey, string siteName, string lang, string filePath, string? blobContainerName = null)
        {
            var key = string.Format(memoryCacheKey, siteName, lang);
            if (!_memoryCache.TryGetValue(key, out T model))
            {
                var containerName = blobContainerName ?? siteName;
                var blobName = string.Format(filePath, lang);
                var jsonContent = await _blobStorageManager.DownloadFile(containerName, blobName);
                model = JsonConvert.DeserializeObject<T>(jsonContent);

                _memoryCache.Set(key, model);
            }

            if (model is PageModel pageModel)
            {
                SearchAndReplaceInPageModel(pageModel, "{{{container}}}", siteName);
            }

            return model;
        }

        public async Task<string> GetStringDataAsync(string memoryCacheKey, string siteName, string lang, string filePath, string? blobContainerName = null)
        {
            var key = string.Format(memoryCacheKey, siteName, lang);
            if (!_memoryCache.TryGetValue(key, out string data))
            {
                var containerName = blobContainerName ?? siteName;
                var blobName = string.Format(filePath, lang);
                var content = await _blobStorageManager.DownloadFile(containerName, blobName);
                data = content;

                _memoryCache.Set(key, data);
            }

            return data;
        }

        public async Task SaveDataAsync<T>(T model, string memoryCacheKey, string siteName, string lang, string filePath)
        {
            T modelCopy = ObjectHelpers.DeepCopy(model);

            if (modelCopy is PageModel pageModelCopy)
            {
                SearchAndReplaceInPageModel(pageModelCopy, siteName, "{{{container}}}");
            }

            var jsonModel = JsonConvert.SerializeObject(modelCopy);
            var blobName = string.Format(filePath, lang);

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModel)))
            await _blobStorageManager.UploadFile(siteName, blobName, stream);

            var key = string.Format(memoryCacheKey, siteName, lang);
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
