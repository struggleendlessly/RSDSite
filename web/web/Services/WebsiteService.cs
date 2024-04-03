using web.Data;
using web.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace web.Services
{
    public class WebsiteService : IWebsiteService
    {
        private readonly ApplicationDbContext _dbContext;

        public WebsiteService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Website> GetWebsiteByName(string siteName)
        {
            return await _dbContext.Websites.FirstOrDefaultAsync(w => w.Name == siteName);
        }

        public async Task<Website> CreateWebsite(Website website)
        {
            _dbContext.Websites.Add(website);
            await _dbContext.SaveChangesAsync();
            return website;
        }
    }
}
