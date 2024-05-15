using shared.Data;
using shared.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace shared.Managers
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStateManager _stateManager;
        private readonly IMemoryCache _memoryCache;

        public SubscriptionService(ApplicationDbContext dbContext, IStateManager stateManager, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _stateManager = stateManager;
            _memoryCache = memoryCache;
        }

        public async Task<bool> IsWebsiteSubscriptionActiveAsync()
        {
            if (_stateManager.SiteName == StaticStrings.DefaultSiteName)
            {
                return true;
            }

            var key = string.Format(StaticStrings.ActiveWebsiteSubscription, _stateManager.SiteName);
            if (_memoryCache.TryGetValue(key, out bool isSubscriptionActive))
            {
                return isSubscriptionActive;
            }

            var website = await _dbContext.Websites
                .Include(w => w.Subscriptions)
                    .ThenInclude(s => s.SubscriptionModule)
                .FirstOrDefaultAsync(w => w.Name == _stateManager.SiteName);

            if (website == null)
            {
                return false;
            }

            var activeWebsiteSubscription = website.Subscriptions
                .FirstOrDefault(s => s.IsActive && s.SubscriptionModule?.Name == StaticStrings.SubscriptionModuleWebsite);

            bool isActive = activeWebsiteSubscription != null;
            _memoryCache.Set(key, isActive, TimeSpan.FromMinutes(30));

            return isActive;
        }

        public async Task<bool> IsCustomDomainSubscriptionActiveAsync()
        {
            if (_stateManager.SiteName == StaticStrings.DefaultSiteName)
            {
                return true;
            }

            var key = string.Format(StaticStrings.ActiveCustomDomainSubscription, _stateManager.SiteName);
            if (_memoryCache.TryGetValue(key, out bool isSubscriptionActive))
            {
                return isSubscriptionActive;
            }

            var website = await _dbContext.Websites
                .Include(w => w.Subscriptions)
                    .ThenInclude(s => s.SubscriptionModule)
                .FirstOrDefaultAsync(w => w.Name == _stateManager.SiteName);

            if (website == null)
            {
                return false;
            }

            var activeWebsiteSubscription = website.Subscriptions
                .FirstOrDefault(s => s.IsActive && s.SubscriptionModule?.Name == StaticStrings.SubscriptionModuleCustomDomain);

            bool isActive = activeWebsiteSubscription != null;
            _memoryCache.Set(key, isActive, TimeSpan.FromMinutes(30));

            return isActive;
        }
    }
}
