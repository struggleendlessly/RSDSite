using shared.Models.API;

namespace shared.Interfaces.Api
{
    public interface IApiCustomDomainService
    {
        Task SaveCustomDomainAsync(SaveCustomDomainModel model);
        Task<string> CheckCustomDomainVerificationAsync(CheckCustomDomainVerificationModel model);
    }
}
