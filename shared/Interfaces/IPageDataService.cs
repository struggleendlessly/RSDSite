using System;

namespace shared.Interfaces
{
    public interface IPageDataService
    {
        Task<T> GetDataAsync<T>(string memoryCacheKey, string filePath, string? blobContainerName = null);
        Task<string> GetStringDataAsync(string memoryCacheKey, string filePath, string? blobContainerName = null);
        Task SaveDataAsync<T>(T model, string memoryCacheKey, string filePath);
    }
}
