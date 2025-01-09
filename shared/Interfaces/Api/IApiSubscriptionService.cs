using System;

namespace shared.Interfaces.Api
{
    public interface IApiSubscriptionService
    {
        Task<bool> IsWebsiteSubscriptionActiveAsync(string siteName);
        Task<bool> IsCustomDomainSubscriptionActiveAsync(string siteName);
    }
}
