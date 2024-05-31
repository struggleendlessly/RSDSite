using shared.Data.Entities;

namespace shared.Interfaces
{
    public interface IWebsiteService
    {
        Task<List<string>> GetWebsitesNamesAsync();
        Guid GetWebsiteId(string siteName);
        List<Website> GetUserSites(string userId);
        Task<Website> GetWebsiteByName(string siteName);
        Task<Website> CreateWebsite(Website website, string userId);
        Task<Website> UpdateAsync(Website website);
        Task<string> GetSiteDomainAsync(string siteName);
        Task<string> UpdateSiteDomainAsync(string siteName, string domain);
    }
}
