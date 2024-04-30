using shared.Data.Entities;

namespace shared.Interfaces
{
    public interface IWebsiteService
    {
        Task<List<string>> GetWebsitesNamesAsync();
        List<string> GetUserSites(string userId);
        Task<Website> GetWebsiteByName(string siteName);
        Task<Website> CreateWebsite(Website website);
        Task<Website> UpdateAsync(Website website);
    }
}
