using shared.Data.Entities;

namespace shared.Interfaces
{
    public interface IWebsiteService
    {
        Task<List<Website>> GetAllOrCreateAsync(string idpName, Guid idpUserId);
        Task<List<Website>> GetAllAsync(string idpName, Guid idpUserId);
        Task<List<string>> GetWebsitesNamesAsync();
        Guid GetWebsiteId(string siteName);
        List<Website> GetUserSites(string userId);
        Task<Website> GetWebsiteByName(string siteName);
        string GetWebsiteName(string domain);
        Task<Website> CreateAsync(Website website, Guid userId);
        Task<Website> UpdateAsync(Website website);
        Task<string> GetSiteDomainAsync(string siteName);
        Task<string> UpdateSiteDomainAsync(string siteName, string domain);
    }
}
