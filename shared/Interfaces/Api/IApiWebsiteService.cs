using shared.Data.Entities;

namespace shared.Interfaces.Api
{
    public interface IApiWebsiteService
    {
        Task<List<Website>> GetAllAsync();
        Task<Website> GetWebsiteAsync(string siteName);
    }
}
