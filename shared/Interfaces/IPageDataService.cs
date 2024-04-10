using System;

namespace shared.Interfaces
{
    public interface IPageDataService
    {
        Task<T> GetDataAsync<T>(string memoryCacheKey, string filePath);
    }
}
