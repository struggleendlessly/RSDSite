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

        public async Task<List<string>> GetWebsitesNamesAsync()
        {
            return await _dbContext.Websites
                .Select(x => x.Name)
                .ToListAsync();
        }

        public Guid GetWebsiteId(string siteName)
        {
            return _dbContext.Websites.FirstOrDefault(x => x.Name == siteName).Id;
        }

        public List<string> GetUserSites(string userId)
        {
            return _dbContext.Websites
                .Include(w => w.User)
                .Where(w => w.User.Id == userId)
                .Select(w => w.Name)
                .ToList();
        }

        public async Task<Website> GetWebsiteByName(string siteName)
        {
            return await _dbContext.Websites.FirstOrDefaultAsync(w => w.Name == siteName);
        }

        public async Task<Website> CreateWebsite(Website website)
        {
            await _dbContext.Websites.AddAsync(website);
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

        public async Task<string> GetSiteDomainAsync(string siteName)
        {
            var website = await _dbContext.Websites.FirstOrDefaultAsync(w => w.Name == siteName);
            return website.Domain;
        }

        public async Task<string> UpdateSiteDomainAsync(string siteName, string domain)
        {
            var website = await _dbContext.Websites.FirstOrDefaultAsync(w => w.Name == siteName);

            website.Domain = domain;
            await _dbContext.SaveChangesAsync();

            return domain;
        }
    }
}
