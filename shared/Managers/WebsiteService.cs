using shared.Data;
using shared.Interfaces;
using shared.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace shared.Managers
{
    public class WebsiteService : IWebsiteService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly AzureBlobStorageManager _azureBlobStorageManager;

        public WebsiteService(
            ApplicationDbContext dbContext, 
            IUserService userService, 
            AzureBlobStorageManager azureBlobStorageManager)
        {
            _dbContext = dbContext;
            _userService = userService;
            _azureBlobStorageManager = azureBlobStorageManager;
        }

        public async Task<List<Website>> GetAllOrCreateAsync(string idpName, Guid idpUserId)
        {
            var websites = await GetAllAsync(idpName, idpUserId);
            if (websites.Count == 0)
            {
                var user = await _userService.GetAsync(idpName, idpUserId);
                if (user is null)
                {
                    return websites;
                }

                var websiteName = GenerateRandomWebsiteName();
                var website = new Website { Name = websiteName };
                await CreateAsync(website, user.Id);

                websites.Add(website);
            }

            return websites;
        }

        public async Task<List<Website>> GetAllAsync(string idpName, Guid idpUserId)
        {
            return await _dbContext.Websites
                .Where(w => w.Users.Any(u =>
                    (idpName.Equals(StaticStrings.MSAL_IDP_Facebook, StringComparison.InvariantCultureIgnoreCase) && u.FacebookId == idpUserId) ||
                    (idpName.Equals(StaticStrings.MSAL_IDP_Google, StringComparison.InvariantCultureIgnoreCase) && u.GoogleId == idpUserId) ||
                    (idpName.Equals(StaticStrings.MSAL_IDP_Microsoft, StringComparison.InvariantCultureIgnoreCase) && u.MicrosoftId == idpUserId)))
                .ToListAsync();
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
                .Where(w => w.Users.Any(/*u => u.Id == userId*/))
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

        public async Task<Website> CreateAsync(Website website, Guid userId)
        {
            await _dbContext.Websites.AddAsync(website);

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.Websites.Add(website);

            await _dbContext.SaveChangesAsync();

            await _azureBlobStorageManager.CopyAllFilesAsync(StaticStrings.ExampleContainerName, website.Name.ToLower());

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

        private string GenerateRandomWebsiteName()
        {
            var random = new Random();
            var randomNumbers = random.Next(100000, 999999);

            return $"website{randomNumbers}";
        }
    }
}
