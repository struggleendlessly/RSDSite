using System;

namespace shared.Interfaces
{
    public interface ISubscriptionService
    {
        Task<bool> IsWebsiteSubscriptionActiveAsync();
        Task<bool> IsCustomDomainSubscriptionActiveAsync();
    }
}
