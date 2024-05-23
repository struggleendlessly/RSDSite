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

        public async Task<Guid> GetWebsiteIdAsync(string siteName)
        {
            var website = await _dbContext.Websites.FirstOrDefaultAsync(x => x.Name == siteName);
            if (website == null)
            {
                throw new InvalidOperationException($"No website found with the name {siteName}");
            }

            return website.Id;
        }

        public List<string> GetUserSites(string userId)
        {
            return _dbContext.Websites
                .Where(w => w.Users.Any(u => u.Id == userId))
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
