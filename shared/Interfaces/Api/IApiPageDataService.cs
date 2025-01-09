using System.Globalization;

namespace shared.Interfaces.Api
{
    public interface IApiPageDataService
    {
        Task<T> GetDataAsync<T>(string memoryCacheKey, string siteName, CultureInfo lang, string filePath, string? blobContainerName = null);
        Task<T> SaveDataAsync<T>(T model, string memoryCacheKey, string siteName, CultureInfo lang, string filePath);
    }
}
