using System;

namespace shared.Interfaces
{
    public interface IPageDataService
    {
        Task<T> GetDataAsync<T>(string memoryCacheKey, string siteName, string lang, string filePath, string? blobContainerName = null);
        Task<string> GetStringDataAsync(string memoryCacheKey, string siteName, string lang, string filePath, string? blobContainerName = null);
        Task SaveDataAsync<T>(T model, string memoryCacheKey, string siteName, string lang, string filePath);
    }
}
