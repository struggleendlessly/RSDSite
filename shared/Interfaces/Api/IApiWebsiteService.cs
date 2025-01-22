using shared.Data.Entities;

namespace shared.Interfaces.Api
{
    public interface IApiWebsiteService
    {
        Task<List<Website>> GetAllAsync();
        Task<Website> GetWebsiteAsync(string siteName);
        Task<string> GetSiteDomainAsync(string siteName);
        Task<Website> CreateAsync(Website website);
        Task<Website> UpdateAsync(Website website);
    }
}
