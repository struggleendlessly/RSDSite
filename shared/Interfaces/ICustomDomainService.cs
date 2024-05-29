using System;

namespace shared.Interfaces
{
    public interface ICustomDomainService
    {
        Task SaveCustomDomainAsync(string customDomain);
        Task<string> CheckCustomDomainVerificationAsync();
    }
}
