using web.Data;

namespace web.Interfaces
{
    public interface IWebsiteService
    {
        Task<Website> GetWebsiteByName(string siteName);
        Task<Website> CreateWebsite(Website website);
    }
}
