using shared.Data.Entities;

namespace shared.Interfaces
{
    public interface IContactUsMessageService
    {
        Task<List<ContactUsMessage>> GetContactUsMessages(string siteName);
        Task<ContactUsMessage> CreateContactUsMessage(ContactUsMessage message);
    }
}
