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

        public List<Website> GetUserSites(string userId)
        {
            return _dbContext.Websites
                .Where(w => w.Users.Any(u => u.Id == userId))
                .ToList();
        }

        public async Task<Website> GetWebsiteByName(string siteName)
        {
            return await _dbContext.Websites.FirstOrDefaultAsync(w => w.Name == siteName);
        }

        public string GetWebsiteName(string domain)
        {
            return _dbContext.Websites.FirstOrDefault(x => x.Domain == domain).Name;
        }

        public async Task<Website> CreateWebsite(Website website, string userId)
        {
            await _dbContext.Websites.AddAsync(website);

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.Websites.Add(website);

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
