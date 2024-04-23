using Microsoft.EntityFrameworkCore;

using shared.Data;
using shared.Interfaces;
using shared.Data.Entities;

namespace shared.Managers
{
    public class WebsiteService : IWebsiteService
    {
        private readonly ApplicationDbContext _dbContext;

        public WebsiteService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetUserSitesAsync(string userId)
        {
            return await _dbContext.Websites
                .Include(w => w.User)
                .Where(w => w.User.Id == userId)
                .Select(w => w.Name)
                .ToListAsync();
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

        public async Task<Website> UpdateAsync(Website website)
        {
            var existingWebsite = await _dbContext.Websites.FindAsync(website.Id);
            if (existingWebsite != null)
            {
                existingWebsite.Name = website.Name;

                await _dbContext.SaveChangesAsync();
            }

            return existingWebsite;
        }
    }
}
