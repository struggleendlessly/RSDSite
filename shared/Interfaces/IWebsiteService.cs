using shared.Data.Entities;

namespace shared.Interfaces
{
    public interface IWebsiteService
    {
        Task<Website> GetWebsiteByName(string siteName);
        Task<Website> CreateWebsite(Website website);
    }
}
