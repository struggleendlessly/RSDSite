using shared.Data.Entities;

namespace shared.Interfaces.Api
{
    public interface IApiWebsiteService
    {
        Task<Website> GetWebsiteByName(string siteName);
    }
}
