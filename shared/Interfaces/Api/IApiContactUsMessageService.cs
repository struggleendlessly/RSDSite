using shared.Data.Entities;

namespace shared.Interfaces.Api
{
    public interface IApiContactUsMessageService
    {
        Task<List<ContactUsMessage>> GetAllAsync(string siteName);
        Task<ContactUsMessage> CreateAsync(ContactUsMessage message);
    }
}
