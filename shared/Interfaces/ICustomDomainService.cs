using System;

namespace shared.Interfaces
{
    public interface ICustomDomainService
    {
        Task SaveCustomDomainAsync(string siteName, string customDomain);
        Task<string> CheckCustomDomainVerificationAsync(string siteName, string lang);
    }
}
